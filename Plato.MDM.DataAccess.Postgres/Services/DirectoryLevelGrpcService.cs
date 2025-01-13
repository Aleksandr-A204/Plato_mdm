using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Repositories;

namespace Plato.MDM.DataAccess.Postgres.Services
{
    public class DirectoryLevelGrpcService : DirectoryLevelService.DirectoryLevelServiceBase
    {
        private readonly IMdmDirectoryLevelRepository _directoryLevelRepository;
        private readonly ILogger<DirectoryLevelGrpcService> _logger;
        private readonly DirectoryLevelMapper _mapper;

        public DirectoryLevelGrpcService(DirectoryLevelMapper mapper, 
            ILogger<DirectoryLevelGrpcService> logger,
            IMdmDirectoryLevelRepository directoryLevelRepository
            )
        {
            _directoryLevelRepository = directoryLevelRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<ListItemResponse> GetDirectoryLevels(GetDirectoryLevelsRequest request, ServerCallContext context)
        {
            var listLevel = await _directoryLevelRepository.GetAllLevelsAsync()
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Не найдено уровней"));

            _logger.LogInformation($"Количество уровней списка: {listLevel.Count}.");

            var directoryMessage = listLevel.Select(_mapper.MapToMessage).ToList();

            return new ListItemResponse
            {
                Items = { directoryMessage }
            };
        }
    }
}
