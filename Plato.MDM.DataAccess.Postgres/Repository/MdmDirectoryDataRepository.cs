using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Data;
using Plato.MDM.Storage.DTOs;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;
using System.Data.Common;


namespace Plato.MDM.Storage.Repositories
{
    public interface IMdmDirectoryDataRepository
    {
        Task<MdmTableData> GetDirectoryDataSqlAsync(Guid versionId);
        //Task<List<ForeignData>> GetDirectoryRelatedDataSqlAsync(string tableName);
        Task<bool> CreateRecordAsync(string tableName);
        Task<bool> EditDirectoryDataAsync(JObject directoryData);
        Task<bool> DeleteDirectoryDataAsync(DeleteMdmTableDataRequest deleteData);
    }

    public class MdmDirectoryDataRepository : IMdmDirectoryDataRepository
    {
        private readonly MdmDbContext _context;
        private readonly ILogger<MdmDirectoryDataRepository> _logger;

        public MdmDirectoryDataRepository(MdmDbContext context, ILogger<MdmDirectoryDataRepository> logger) : base()
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MdmTableData> GetDirectoryDataSqlAsync(Guid versionId)
        {
            var version = await _context.MdmDirectoryVersions.FindAsync(versionId) 
                ?? throw new Exception("Версия не найдена.");

            if (string.IsNullOrEmpty(version.TableName))
                return new MdmTableData();

            var tableName = version.TableName;

            using var connection = await OpenConnectionAsync();

            var mainTable = await ExecuteQueryAsync(tableName, connection);

            var foreignKeys = await GetForeignKeysAsync(tableName, connection);

            var relatedTables = new Dictionary<string, JArray>();
            foreach (ForeignKeyInfo foreignKey in foreignKeys)
                relatedTables.Add(foreignKey.ColumnName, await ExecuteQueryAsync(foreignKey.ForeignTableName, connection));  
                
            return new MdmTableData { TableName = tableName, MainTable = mainTable, ForeignTables = relatedTables };
        }

        public async Task<bool> CreateRecordAsync(string tablename)
        {
            using var connection = await OpenConnectionAsync();

            if (!await CheckTableExistsAsync(tablename, connection))
            {
                _logger.LogWarning($"Таблица '{tablename}' не найдена в базе данных.");
                return false;
            }

            var newId = Guid.NewGuid();

            var sql = $@"INSERT INTO municipality_ko(""InstanceId"") VALUES('{newId}')";

            return await connection.ExecuteScalarAsync<int>(sql) > 0;
        }

        public async Task<bool> EditDirectoryDataAsync(JObject data)
        {
            if (data == null)
                return false;

            var tablename = data.First?.Path;
            if (tablename == null)
                return false;

            using var connection = await OpenConnectionAsync();

            if (!await CheckTableExistsAsync(tablename, connection))
            {
                _logger.LogWarning($"Таблица '{tablename}' не найдена в базе данных.");
                return false;
            }

            var columnsInfo = await GetColumnsInfoAsync(tablename, connection);

            var columnNamesWithUdt = new string[columnsInfo.Count];
            var columnNamesWithoutUdt = new string[columnsInfo.Count];

            for (int i = 0; i < columnsInfo.Count; i++)
            {
                var columnName = columnsInfo[i].ColumnName;
                columnNamesWithUdt[i] = $@"""{columnName}"" {columnsInfo[i].UdtName}";
                if (columnsInfo[i].UdtName != "InstanceId")
                    columnNamesWithoutUdt[i] = $"\"{columnName}\"";
            }

            var sql = $@"UPDATE ""{tablename}"" as mt SET ({string.Join(", ", columnNamesWithoutUdt)}) = ({string.Join(", ", columnNamesWithoutUdt.Select(x => $"md.{x}"))})
                        FROM (select * from json_to_recordset('{data[tablename]}')
	                    as x({string.Join(", ", columnNamesWithUdt)})) as md
	                    where mt.""InstanceId"" = md.""InstanceId"";";

            _logger.LogInformation($"Сгенерированный запрос: {sql}");


            return await connection.ExecuteScalarAsync<int>(sql) > 0;
        }

        public async Task<bool> DeleteDirectoryDataAsync(DeleteMdmTableDataRequest deleteData)
        {
            using var connection = await OpenConnectionAsync();

            var db = new QueryFactory(connection, new PostgresCompiler());

            return await db.Query(deleteData.Tablename).WhereIn("InstanceId", deleteData.Ids).DeleteAsync() > 0;
        }

        private async Task<bool> CheckTableExistsAsync(string tableName, DbConnection connection)
        {
            var sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM information_schema.tables 
                    WHERE table_schema = 'public' 
                    AND table_name = @TableName );";

            return await connection.ExecuteScalarAsync<bool>(sql, new { TableName = tableName });
        }

        private async Task<List<ColumnsInfo>> GetColumnsInfoAsync(string tableName, DbConnection connection)
        {
            var sql = @"SELECT column_name as ""ColumnName"", udt_name as ""UdtName"" 
                        FROM information_schema.columns 
                        WHERE table_name = @Tablename and udt_name != 'geometry';";

            _logger.LogInformation($"Соединение с базой данных открыто. Имя таблицы: {tableName}");

            var columnsInfo = await connection.QueryAsync<ColumnsInfo>(sql, new { Tablename = tableName });
            _logger.LogInformation($"Запрос выполнен. Найдено количество столбцов: {columnsInfo.Count()}");
            return columnsInfo.ToList();
        }

        private async Task<List<ForeignKeyInfo>> GetForeignKeysAsync(string tableName, DbConnection connection)
        {
            var sql = @"
                SELECT kcu.column_name AS ""ColumnName"", ccu.table_name AS ""ForeignTableName"", ccu.column_name AS ""ForeignColumnName""
                FROM information_schema.table_constraints AS tc 
                JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name
                JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name
                WHERE tc.constraint_type = 'FOREIGN KEY' AND kcu.table_name = @Tablename;";

            _logger.LogInformation($"Соединение с базой данных открыто. Имя таблицы: {tableName}");

            var foreignKeys = await connection.QueryAsync<ForeignKeyInfo>(sql, new { Tablename = tableName });
            _logger.LogInformation($"Запрос выполнен. Найдено внешних ключей: {foreignKeys.Count()}");
            return foreignKeys.ToList();
        }

        private async Task<JArray> ExecuteQueryAsync(string tableName, DbConnection connection)
        {
            var sql = $@"
                    SELECT jsonb_agg(
                        row_to_json(md)) as objects
                    FROM ""{tableName}"" md";


            _logger.LogInformation($"Сгенерированный запрос: {sql}");

            var inputArray = JArray.Parse(await connection.ExecuteScalarAsync<string>(sql) ?? string.Empty);
            var outputArray = new JArray();

            foreach (JObject item in inputArray)
            {
                item.Remove("geom");
                outputArray.Add(item);
            }

            return outputArray;
        }

        private async Task<DbConnection> OpenConnectionAsync()
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            return connection;
        }

        private class ColumnsInfo
        {
            public string? ColumnName { get; set; }
            public string? UdtName { get; set; }
        }

        private class ForeignKeyInfo
        {
            public required string ColumnName { get; set; }
            public required string ForeignTableName { get; set; }
            public required string ForeignColumnName { get; set; }
        }
    }
}
