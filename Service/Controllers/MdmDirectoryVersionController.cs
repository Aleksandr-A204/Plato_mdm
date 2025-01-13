using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.DTOs;
using System.ComponentModel;
using static Plato.MDM.DataAccess.Postgres.Protos.DirectoryVersionService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Plato.MDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MdmDirectoryVersionController : ControllerBase
    {
        private readonly DirectoryVersionServiceClient _directoryVersionGrpcService;
        private readonly ILogger<MdmDirectoryVersionController> _logger;
        private readonly IMapper _mapper;

        public MdmDirectoryVersionController(ILogger<MdmDirectoryVersionController> logger, 
            DirectoryVersionServiceClient directoryVersionGrpcService,
            IMapper mapper)
        {
            _logger = logger;
            _directoryVersionGrpcService = directoryVersionGrpcService;
            _mapper = mapper;
        }

        [HttpGet]
        [Description("�������� ������ �����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllVersions()
        {
            try
            {
                var data = await _directoryVersionGrpcService.GetVersionsByDirectoryAsync(new() { Id = "" });
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ��������� ������������.");
                return BadRequest("�� ������� �������� �����������.");
            }
        }

        [HttpGet("{id}")]
        [Description("�������� ������ ����������� �� ��� id � ��������� � �������� ������� �� �������� VersionDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllVersionsByDirectory(string id)
        {
            try
            {
                var responseGrpc = await _directoryVersionGrpcService.GetVersionsByDirectoryAsync(new() { Id = id });
                return Ok(responseGrpc.DirectoryVersions.Select(_mapper.Map<MdmDirectoryVersionDto>));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ��������� ������������.");
                return BadRequest("�� ������� �������� �����������.");
            }
        }

        [HttpPost]
        [Description("��������� ������ �����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDirectoryVersion([FromBody] MdmDirectoryVersionDto versionDto)
        {
            if (versionDto == null)
                return BadRequest("������������ ������.");

            try
            {
                var directoryVersion = _mapper.Map<VersionByDirectoryReply>(versionDto);

                var responseGrpc = await _directoryVersionGrpcService.CreateDirectoryVersionAsync(directoryVersion);

                if(responseGrpc.Success)
                    return Ok(responseGrpc.Message);

                return BadRequest("������������ ������.");
                //return CreatedAtAction(nameof(GetAllVersionsByDirectory), new { id = version.Id }, version);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ���������� ������ �����������.");
                return BadRequest("�� ������� �������� ������ ����������.");
            }
        }

        [HttpPut]
        [Description("����������� ������ �����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] MdmDirectoryVersionDto updatedVersionDto)
        {
            if (updatedVersionDto == null)
                return BadRequest("������������ ������.");

            try
            {
                var directoryVersion = _mapper.Map<VersionByDirectoryReply>(updatedVersionDto);

                var responseGrpc = await _directoryVersionGrpcService.UpdateDirectoryVersionAsync(directoryVersion);

                if (responseGrpc.Success)
                    return NoContent();

                return BadRequest(responseGrpc.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� �������������� �����������.");
                return BadRequest("�� ������� ������������� ����������.");
            }
        }

        [HttpDelete("{id}")]
        [Description("������� ������ �����������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _directoryVersionGrpcService.DeleteDirectoryVersionAsync(new() { Id = id });
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
