using AutoMapper;
using GardenControlApi.Models;
using GardenControlServices;
using GardenControlServices.Interfaces;
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
    public class DS18B20Controller : ControllerBase
    {
        private DS18B20Service _dS18B20Service { get; init; }
        private ILogger<DS18B20Controller> _logger { get; init; }
        private IMapper _mapper { get; init; }

        public DS18B20Controller(DS18B20Service dS18B20Service, ILogger<DS18B20Controller> logger, IMapper mapper)
        {
            _dS18B20Service = dS18B20Service;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a reading value from the specified DS18B20 temperature probe
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the current temperature from the DS18B20 sensor</returns>
        /// <response code="200">Returns temperature value</response>
        /// <response code="404">Could not find DS18B20 probe from id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DS18B20Dto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DS18B20Dto> Get(int id)
        {
            var controlDeviceReading = await _dS18B20Service.GetTemperatureReading(id);

            if (controlDeviceReading == null)
                throw new Exception("Unable to take measurement");

            var response = _mapper.Map<DS18B20Dto>(controlDeviceReading);


            return response;
        }

        /// <summary>
        /// Returns the serial numbers of all the attached probes to the specified GPIO Pin.
        /// Useful for getting serial numbers to register each probe.
        /// </summary>
        /// <returns>List of serial numbers</returns>
        /// <response code="200">Returns a list of the 1-wire protocol serial numbers attached to the GPIO pin</response>
        [HttpGet("SerialNumbers/{gpioPin}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public async Task<List<string>> GetSerialNumbers(int gpioPin)
        {
            var serialNumbers = await _dS18B20Service.GetSerialNumbers(gpioPin);

            return serialNumbers;
        }
    }
}
