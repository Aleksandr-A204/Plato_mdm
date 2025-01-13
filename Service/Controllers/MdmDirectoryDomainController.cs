using Microsoft.AspNetCore.Mvc;
using Plato.MDM.DataAccess.Postgres.Protos;
using System.ComponentModel;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryDomainService;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryLevelService;

namespace Plato.MDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MdmDirectoryDomainController : ControllerBase
    {
        private readonly ILogger<MdmDirectoryDomainController> _logger;
        private readonly DirectoryDomainServiceClient _directoryDomainService;

        public MdmDirectoryDomainController(ILogger<MdmDirectoryDomainController> logger, DirectoryDomainServiceClient directoryDomainService)
        {
            _logger = logger;
            _directoryDomainService = directoryDomainService;
        }

        [HttpGet]
        [Description("Получает все предметные области справочника")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDomains()
        {
            try
            {
                var data = await _directoryDomainService.GetDirectoryDomainsAsync(new());
                return Ok(data.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении предметных областей справочника.");
                return BadRequest("Не удалось получить предметные области справочника.");
            }
        }

    }
}
