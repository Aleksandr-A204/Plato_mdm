using Grpc.Core;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Repositories;
using System.Data;

namespace Plato.MDM.DataAccess.Postgres.Services
{
    public class DirectoryVersionGrpcService : DirectoryVersionService.DirectoryVersionServiceBase
    {
        private readonly IMdmDirectoryVersionRepository _directoryVersionRepository;
        private readonly ILogger<DirectoryVersionGrpcService> _logger;
        private readonly DirectoryVersionMapper _mapper;

        public DirectoryVersionGrpcService(ILogger<DirectoryVersionGrpcService> logger, 
            IMdmDirectoryVersionRepository directoryVersionRepository,
            DirectoryVersionMapper mapper)
        {
            _directoryVersionRepository = directoryVersionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<ListVersionByDirectoryResponse> GetVersionsByDirectory(IdRequest request, ServerCallContext context)
        {
            var listVersion = await _directoryVersionRepository.GetAllVersionsByDirectoryAsync(Guid.Parse(request.Id))
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Не найдено содержимых справочника"));

            _logger.LogInformation($"Количество версии выбранного справочника: {listVersion.Count}.");

            var versionMessage = listVersion.Select(_mapper.MapToMessage).ToList();

            return new()
            {
                DirectoryVersions = { versionMessage }
            };
        }

        public override async Task<MessageResponse> CreateDirectoryVersion(VersionByDirectoryReply request, ServerCallContext context)
        {
            var directoryVersionEntity = _mapper.MapToEntity(request);

            if (await _directoryVersionRepository.AddVersionAsync(directoryVersionEntity))
                return new() { Success = true };

            return new() { Success = false };
        }

        public override async Task<MessageResponse> UpdateDirectoryVersion(VersionByDirectoryReply request, ServerCallContext context)
        {
            var directoryVersionEntity = _mapper.MapToEntity(request);

            if (await _directoryVersionRepository.EditVersionAsync(directoryVersionEntity))
                return new() { Success = true };

            return new() { Success = false };
        }

        public override async Task<MessageResponse> DeleteDirectoryVersion(IdRequest request, ServerCallContext context)
        {
            var isDeleted = await _directoryVersionRepository.DeleteVersionAsync(Guid.Parse(request.Id));

            //_logger.LogInformation($"Directory is retrived for {coupon.ProductName}.");

            return new() { Message = "Успешно удалена версия выбранного справочника.", Success = true };
        }
    }
}
