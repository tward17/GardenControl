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

        [HttpGet("{id}")]
        public async Task<RelayDto> Get(int id)
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

        [HttpPost]
        public async Task Post(RelaySetStateDto relaySetState)
        {
            await _relayService.SetRelayState(relaySetState.DeviceId, relaySetState.State);
        }
    }
}
