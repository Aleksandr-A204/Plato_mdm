using Grpc.Core;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.Repositories;

namespace Plato.MDM.DataAccess.Postgres.Services
{
    public class DirectoryGrpcService : DirectoryService.DirectoryServiceBase
    {
        private readonly IMdmDirectoryRepository _directoryRepository;
        private readonly ILogger<DirectoryGrpcService> _logger;
        private readonly DirectoryMapper _mapper;

        public DirectoryGrpcService(IMdmDirectoryRepository directoryRepository, ILogger<DirectoryGrpcService> logger, DirectoryMapper mapper)
        {
            _directoryRepository = directoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<GetDirectoriesResponse> GetDirectories(GetDirectoriesRequest request, ServerCallContext context)
        {
            var listDirectories = await _directoryRepository.GetAllDirectoriesAsync()
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Не найдено справочников"));

            _logger.LogInformation($"Количество справочников списка: {listDirectories.Count}.");

            var directoryMessage = listDirectories.Select(_mapper.MapToMessage).ToList();

            return new()
            {
                Directories = { directoryMessage }
            };
        }

        public override async Task<MessageResponse> CreateDirectory(CreateDirectoryRequest request, ServerCallContext context)
        {
            var directoryEntity = _mapper.MapToEntity(request.Directory);

            if (!await _directoryRepository.CreateDirectoryAsync(directoryEntity))
                return new() { Success = false };

            return new() { Success = true };
        }

        public override async Task<MessageResponse> UpdateDirectory(UpdateDirectoryRequest request, ServerCallContext context)
        {
            var directoryEntity = _mapper.MapToEntity(request.Directory);

            if (!await _directoryRepository.UpdateDirectoryAsync(directoryEntity))
                return new() { Success = false };

            return new() { Success = true };
        }

        public override async Task<MessageResponse> DeleteDirectory(IdRequest request, ServerCallContext context)
        {
            await _directoryRepository.DeleteDirectoryAsync(Guid.Parse(request.Id));

            return new() { Message = "Успешно удален справочник.", Success = true };
        }
    }
}
