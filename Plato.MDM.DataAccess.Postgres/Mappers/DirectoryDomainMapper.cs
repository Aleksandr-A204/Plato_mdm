using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Models;
using Riok.Mapperly.Abstractions;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    [Mapper]
    public partial class DirectoryDomainMapper
    {
        public ItemReply MapToMessage(MdmDirectoryDomainEntity directoryDomain)
            => new()
            {
                Id = directoryDomain.Id.ToString(),
                Name = directoryDomain.Name
            };
    }
}
