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
    public class FloatSensorController : ControllerBase
    {
        private FloatSensorService _floatSensorService { get; init; }
        private ILogger<FloatSensorController> _logger { get; init; }
        private IMapper _mapper { get; init; }

        public FloatSensorController(FloatSensorService flaotSensorService, ILogger<FloatSensorController> logger, IMapper mapper)
        {
            _floatSensorService = flaotSensorService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<FloatSensorDto> Get(int id)
        {
            var controlDeviceReading = await _floatSensorService.GetFloatSensorState(id);

            if (controlDeviceReading == null)
                throw new Exception("Unable to take float sensor reading");

            var response = new FloatSensorDto
            {
                FloatSensorState = Enum.GetName(controlDeviceReading)
            };

            return response;
        }
    }
}
