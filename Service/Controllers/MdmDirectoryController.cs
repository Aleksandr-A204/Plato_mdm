using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.DTOs;
using System.ComponentModel;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryService;

namespace Plato.MDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MdmDirectoryController : ControllerBase
    {
        private readonly ILogger<MdmDirectoryController> _logger;
        private readonly DirectoryServiceClient _directoryGrpcClient;
        private readonly IMapper _mapper;

        public MdmDirectoryController(ILogger<MdmDirectoryController> logger, DirectoryServiceClient directoryGrpcClient, IMapper mapper)
        {
            _logger = logger;
            _directoryGrpcClient = directoryGrpcClient;
            _mapper = mapper;
        }

        [HttpGet]
        [Description("Получает все справочники")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDirectories()
        {          
            try
            {
                var data = await _directoryGrpcClient.GetDirectoriesAsync(new());
                return Ok(data.Directories.Select(_mapper.Map<MdmDirectoryDto>).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении справочников.");
                return BadRequest("Не удалось получить справочники.");
            }

        }

        [HttpGet("{id}")]
        [Description("Получает выбранный справочник")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetDirectoryById(Guid id)
        {
            try
            {
                // _mapper.Map<MdmDirectoryDto>(await _mdmRepository.GetDirectoryByIdAsync(id))
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении справочников.");
                return BadRequest("Не удалось получить справочники.");
            }
        }

        //[HttpGet("SearchKeyWord/{searchKeyWord}")]
        //[Description("")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult> GetFilteredDirectories(string searchKeyWord)
        //{
        //    try
        //    {
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Ошибка при получении справочников.");
        //        return BadRequest("Не удалось получить справочники.");
        //    }
        //}

        [HttpPost]
        [Description("Добавляет справочник")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDirectory([FromBody] MdmDirectoryDto directoryDto)
        {
            if (directoryDto == null)
                return BadRequest("Некорректные данные.");

            try
            {
                var directory = _mapper.Map<DirectoryReply>(directoryDto);

                var data = await _directoryGrpcClient.CreateDirectoryAsync(new() { Directory = directory });

                if (data.Success)
                    return CreatedAtAction(nameof(GetAllDirectories), new { id = directory.Id }, directory);

                return BadRequest("Не удалось добавить справочник.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении справочника.");
                return BadRequest("Не удалось добавить справочник.");
            }
        }

        [HttpPut]
        [Description("Редактирует справочник")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] MdmDirectoryDto directoryDto)
        {
            if (directoryDto == null)
                return BadRequest("Некорректные данные.");

            try
            {
                var directory = _mapper.Map<DirectoryReply>(directoryDto);

                var data = await _directoryGrpcClient.UpdateDirectoryAsync(new() { Directory = directory });

                if (data.Success)
                    return NoContent();

                return BadRequest("Не удалось обновить справочник.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при редактировании справочника.");
                return BadRequest("Не удалось редактировать справочник.");
            }
        }

        [HttpDelete("{id}")]
        [Description("Удаляет справочник")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _directoryGrpcClient.DeleteDirectoryAsync(new() { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении справочника.");
                return BadRequest("Не удалось удалить справочник.");
            }
        }
    }
}