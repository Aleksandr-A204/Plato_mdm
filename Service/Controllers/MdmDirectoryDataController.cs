using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using Plato.MDM.Storage.DTOs;
using System.ComponentModel;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryDataService;


namespace Plato.MDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryDataController : ControllerBase
    {
        private readonly ILogger<DirectoryDataController> _logger;
        private readonly DirectoryDataServiceClient _grpcService;
        private readonly IMapper _mapper;

        public DirectoryDataController(ILogger<DirectoryDataController> logger, DirectoryDataServiceClient grpcService, IMapper mapper)
        {
            _logger = logger;
            _grpcService = grpcService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDirectoryDataSqlAsync(string id)
        {
            try
            {
                var dataResponse = await _grpcService.GetDirectoryDataAsync(new() { Id = id });

                return Ok(_mapper.Map<MdmTableDataDto>(dataResponse));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
            }
        }

        //[HttpGet("/RelatedData/{tablename}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetRelatedDataAsync(string tablename)
        //{
        //    try
        //    {
        //        return Ok(await _repository.GetDirectoryRelatedDataSqlAsync(tablename));
        //    }
        //    catch (PostgresException ex) when (ex.SqlState == "42P01") // Код ошибки для "не существует таблицы"
        //    {
        //        return NotFound($"Таблица не найдена: {ex.MessageText}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
        //    }
        //}

        [HttpPost("{tablename}")]
        [Description("Создавает")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecordAsync(string tablename)
        {
            try
            {
                var response = await _grpcService.CreateDirectoryDataAsync(new() { Name = tablename });

                if(response.Success)
                    return Ok();

                return BadRequest("Не удалось добавить справочник.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
            }
        }

        [HttpPut]
        [Description("Редактировает выбранные записи справочника")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditDirectoryDataAsync([FromBody] JObject directoryData)
        {
            try
            {
                var dataResponse = await _grpcService.UpdateDirectoryDataAsync(new() { StringJson = directoryData.ToString() });

                return Ok();
            }
            catch (PostgresException ex) when (ex.SqlState == "42P01") // Код ошибки для "не существует таблицы"
            {
                return NotFound($"Таблица не найдена: {ex.MessageText}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
            }
        }

        [HttpDelete]
        [Description("Удаляет записи из справочника по указанным идентификаторам")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDirectoryDataSqlAsync([FromBody] JObject deleteData)
        {
            if (deleteData == null)
                return BadRequest("Необходимо указать идентификаторы для удаления.");

            try
            {
                var dataResponse = await _grpcService.DeleteDirectoryDataAsync(new() { StringJson  = deleteData.ToString() });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
            }
        }
    }
}
