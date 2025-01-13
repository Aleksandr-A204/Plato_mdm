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
        [Description("�������� ��� �����������")]
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
                _logger.LogError(ex, "������ ��� ��������� ������������.");
                return BadRequest("�� ������� �������� �����������.");
            }

        }

        [HttpGet("{id}")]
        [Description("�������� ��������� ����������")]
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
                _logger.LogError(ex, "������ ��� ��������� ������������.");
                return BadRequest("�� ������� �������� �����������.");
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
        //        _logger.LogError(ex, "������ ��� ��������� ������������.");
        //        return BadRequest("�� ������� �������� �����������.");
        //    }
        //}

        [HttpPost]
        [Description("��������� ����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDirectory([FromBody] MdmDirectoryDto directoryDto)
        {
            if (directoryDto == null)
                return BadRequest("������������ ������.");

            try
            {
                var directory = _mapper.Map<DirectoryReply>(directoryDto);

                var data = await _directoryGrpcClient.CreateDirectoryAsync(new() { Directory = directory });

                if (data.Success)
                    return CreatedAtAction(nameof(GetAllDirectories), new { id = directory.Id }, directory);

                return BadRequest("�� ������� �������� ����������.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ���������� �����������.");
                return BadRequest("�� ������� �������� ����������.");
            }
        }

        [HttpPut]
        [Description("����������� ����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] MdmDirectoryDto directoryDto)
        {
            if (directoryDto == null)
                return BadRequest("������������ ������.");

            try
            {
                var directory = _mapper.Map<DirectoryReply>(directoryDto);

                var data = await _directoryGrpcClient.UpdateDirectoryAsync(new() { Directory = directory });

                if (data.Success)
                    return NoContent();

                return BadRequest("�� ������� �������� ����������.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� �������������� �����������.");
                return BadRequest("�� ������� ������������� ����������.");
            }
        }

        [HttpDelete("{id}")]
        [Description("������� ����������")]
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
                _logger.LogError(ex, "������ ��� �������� �����������.");
                return BadRequest("�� ������� ������� ����������.");
            }
        }
    }
}