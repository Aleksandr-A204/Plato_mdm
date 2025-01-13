using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.DTOs;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    public class DirectoryDataMapper
    {
        public DirectoryDataResponse MapToMessage(MdmTableData directoryData)
        {
            if (directoryData == null) 
                throw new ArgumentNullException(nameof(directoryData));

            var relatedTables = new Dictionary<string, string>();
            foreach (var data in directoryData.ForeignTables)
                relatedTables.Add(data.Key, data.Value.ToString());

            return new DirectoryDataResponse
            {
                TableName = directoryData.TableName,
                MainTable = directoryData.MainTable.ToString(),
                ForeignTables = { relatedTables }
            };
        }

        //public MdmTableData MapToEntity(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //        throw new ArgumentNullException(nameof(data));

        //    var relatedTables = new Dictionary<string, JArray>();
        //    foreach (var data in directoryData.ForeignTables)
        //        relatedTables.Add(data.Key, data.Value.ToString());

        //    return new DirectoryDataReply
        //    {
        //        TableName = directoryData.TableName,
        //        MainTable = directoryData.MainTable.ToString(),
        //        ForeignTables = { relatedTables }
        //    };
        //}
    }
}
