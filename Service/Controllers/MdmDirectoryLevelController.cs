using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryLevelService;

namespace Plato.MDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MdmDirectoryLevelController : ControllerBase
    {
        private readonly ILogger<MdmDirectoryLevelController> _logger;
        private readonly DirectoryLevelServiceClient _directoryLevelGrpcClient;

        public MdmDirectoryLevelController(ILogger<MdmDirectoryLevelController> logger, DirectoryLevelServiceClient directoryLevelGrpcClient)
        {
            _logger = logger;
            _directoryLevelGrpcClient = directoryLevelGrpcClient;
        }

        [HttpGet]
        [Description("Получает все уровни справочника")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllLevels()
        {
            try
            {
                var list = await _directoryLevelGrpcClient.GetDirectoryLevelsAsync(new());
                return Ok(list.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении уровней справочника.");
                return BadRequest("Не удалось получить уровни справочника.");
            }
        }
    }
}