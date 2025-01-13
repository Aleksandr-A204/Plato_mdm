using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Repositories;

namespace Plato.MDM.DataAccess.Postgres.Services
{
    public class DirectoryDomainGrpcService : DirectoryDomainService.DirectoryDomainServiceBase
    {
        private readonly IMdmDirectoryDomainRepository _directoryDomainRepository;
        private readonly ILogger<DirectoryDomainGrpcService> _logger;
        private readonly DirectoryDomainMapper _mapper;

        public DirectoryDomainGrpcService(IMdmDirectoryDomainRepository directoryDomainRepository, 
            ILogger<DirectoryDomainGrpcService> logger,
            DirectoryDomainMapper mapper
            )
        {
            _directoryDomainRepository = directoryDomainRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<ListItemResponse> GetDirectoryDomains(GetDirectoryDomainsRequest request, ServerCallContext context)
        {
            var listDomain = await _directoryDomainRepository.GetAllDomainsAsync()
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Не найдено содержимых справочника"));

            _logger.LogInformation($"Количество domains списка: {listDomain.Count}.");

            var directoryMessage = listDomain.Select(_mapper.MapToMessage).ToList();

            return new ListItemResponse
            {
                Items = { directoryMessage }
            };
        }
    }
}
