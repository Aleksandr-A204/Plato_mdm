using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Models;
using Riok.Mapperly.Abstractions;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    [Mapper]
    public partial class DirectoryVersionMapper
    {
        public MdmDirectoryVersionEntity MapToEntity(VersionByDirectoryReply directoryVersion)
            => new()
            {
                Id = Guid.Parse(directoryVersion.Id),
                DirectoryId = Guid.Parse(directoryVersion.DirectoryId),
                DataSourceName = string.IsNullOrEmpty(directoryVersion.DataSourceName) ? null : directoryVersion.DataSourceName,
                DataSourceDate = string.IsNullOrEmpty(directoryVersion.DataSourceDate) ? null : directoryVersion.DataSourceDate,
                DataSourceUrl = string.IsNullOrEmpty(directoryVersion.DataSourceUrl) ? null : directoryVersion.DataSourceUrl,
                Version = directoryVersion.Version,
                VersionDescription = string.IsNullOrEmpty(directoryVersion.VersionDescription) ? null : directoryVersion.VersionDescription,
                TableName = string.IsNullOrEmpty(directoryVersion.TableName) ? null : directoryVersion.TableName,
                VersionDate = string.IsNullOrEmpty(directoryVersion.VersionDate) ? (DateOnly?)null : DateOnly.Parse(directoryVersion.VersionDate)
            };

        public VersionByDirectoryReply MapToMessage(MdmDirectoryVersionEntity directoryVersion)
            => new()
            {
                Id = directoryVersion.Id.ToString(),
                DirectoryId = directoryVersion.DirectoryId?.ToString() ?? string.Empty,
                DataSourceName = directoryVersion.DataSourceName ?? string.Empty,
                DataSourceDate = directoryVersion.DataSourceDate ?? string.Empty,
                DataSourceUrl = directoryVersion.DataSourceUrl ?? string.Empty,
                Version = directoryVersion.Version,
                VersionDate = directoryVersion.VersionDate.ToString() ?? string.Empty,
                VersionDescription = directoryVersion.VersionDescription ?? string.Empty,
                TableName = directoryVersion.TableName ?? string.Empty
            };
    }
}
