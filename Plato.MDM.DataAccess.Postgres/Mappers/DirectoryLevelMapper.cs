using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Models;
using Riok.Mapperly.Abstractions;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    [Mapper]
    public partial class DirectoryLevelMapper
    {
        public ItemReply MapToMessage(MdmDirectoryLevelEntity directoryLevel)
            => new()
            {
                Id = directoryLevel.Id.ToString(),
                Name = directoryLevel.Name
            };
    }
}
