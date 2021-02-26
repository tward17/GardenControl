using AutoMapper;
using GardenControlApi.Models;
using GardenControlServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Controllers.Devices
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelayController : ControllerBase
    {
        private RelayService _relayService { get; init; }
        private ILogger<RelayController> _logger { get; init; }
        private IMapper _mapper { get; init; }

        public RelayController(RelayService relayService, ILogger<RelayController> logger, IMapper mapper)
        {
            _relayService = relayService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns the current state of the relay
        /// </summary>
        /// <returns>Returns the current state of the relay</returns>
        /// <response code="200">Returns relay state</response>
        /// <response code="404">Could not find relay from id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RelayDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<RelayDto> Get([FromRoute] int id)
        {
            var controlDeviceReading = await _relayService.GetRelayState(id);

            if (controlDeviceReading == null)
                throw new Exception("Unable to take relay reading");

            var response = new RelayDto
            {
                RelayState = Enum.GetName(controlDeviceReading)
            };

            return response;
        }

        /// <summary>
        /// Updates the current state of the relay
        /// </summary>
        /// <returns>Updates the current state of the relay</returns>
        /// <response code="204">Relay state updated successfully</response>
        /// <response code="400">Relay Id in url and object do not match</response>
        /// <response code="404">Could not find relay from id</response>
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromRoute] int id, [FromBody] RelaySetStateDto relaySetState)
        {
            if (id != relaySetState.DeviceId)
                return BadRequest();

            await _relayService.SetRelayState(relaySetState.DeviceId, relaySetState.State);

            return NoContent();
        }
    }
}
