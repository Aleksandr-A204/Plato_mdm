using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Models;
using Riok.Mapperly.Abstractions;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    [Mapper]
    public partial class DirectoryMapper
    {
        public MdmDirectoryEntity MapToEntity(DirectoryReply directory)
            => new()
            {
                Id = Guid.Parse(directory.Id),
                Name = directory.Name,
                Description = string.IsNullOrEmpty(directory.Description) ? null : directory.Description,
                DirectoryDomainId = ParseNullableGuid(directory.DirectoryDomainId),
                DirectoryLevelId = ParseNullableGuid(directory.DirectoryLevelId)
            };

        public DirectoryReply MapToMessage(MdmDirectoryEntity directory)
            => new()
            {
                Id = directory.Id.ToString(),
                Name = directory.Name,
                Description = directory.Description ?? string.Empty,
                DirectoryDomainId = directory.DirectoryDomainId.ToString(),
                DirectoryLevelId = directory.DirectoryLevelId.ToString()
            };

        private Guid? ParseNullableGuid(string? guidString) 
            => string.IsNullOrEmpty(guidString) ? (Guid?)null : Guid.Parse(guidString);
    }
}
