using Grpc.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.DTOs;
using Plato.MDM.Storage.Repositories;

namespace Plato.MDM.DataAccess.Postgres.Services
{
    public class DirectoryDataGrpcService : DirectoryDataService.DirectoryDataServiceBase
    {
        private readonly IMdmDirectoryDataRepository _directoryDataRepository;
        private readonly ILogger<DirectoryDataGrpcService> _logger;
        private readonly DirectoryDataMapper _mapper;

        public DirectoryDataGrpcService(IMdmDirectoryDataRepository directoryDataRepository,
            ILogger<DirectoryDataGrpcService> logger,
            DirectoryDataMapper mapper
            )
        {
            _directoryDataRepository = directoryDataRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<DirectoryDataResponse> GetDirectoryData(IdRequest request, ServerCallContext context)
        {
            var directoryData = await _directoryDataRepository.GetDirectoryDataSqlAsync(Guid.Parse(request.Id))
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Не найдено данных справочника"));

            return _mapper.MapToMessage(directoryData);
        }

        public override async Task<MessageResponse> CreateDirectoryData(ItemReply request, ServerCallContext context)
        {
            if (!await _directoryDataRepository.CreateRecordAsync(request.Name))
                return new() { Success = false };

            // _logger.LogInformation($"Успешно  {request.Name}.");

            return new() { Success = true };
        }

        public override async Task<MessageResponse> UpdateDirectoryData(DataRequest request, ServerCallContext context)
        {
            bool success = await _directoryDataRepository.EditDirectoryDataAsync(JObject.Parse(request.StringJson));

            if (success)
                return new MessageResponse { Success = true };

            return new MessageResponse { Success = false };
        }

        public override async Task<MessageResponse> DeleteDirectoryData(DataRequest request, ServerCallContext context)
        {
            var mappedData = JsonConvert.DeserializeObject<DeleteMdmTableDataRequest>(request.StringJson);

            bool success = await _directoryDataRepository.DeleteDirectoryDataAsync(mappedData);

            if(!success)
                return new MessageResponse { Success = false };

            _logger.LogInformation($"Success deleted data.");

            return new MessageResponse { Success = true };
        }
    }
}
