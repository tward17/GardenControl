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
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<DS18B20Dto> Get(int id)
        {
            var controlDeviceReading = await _dS18B20Service.GetTemperatureReading(id);

            if (controlDeviceReading == null)
                throw new Exception("Unable to take measurement");

            var response = _mapper.Map<DS18B20Dto>(controlDeviceReading);


            return response;
        }

        ///// <summary>
        ///// Returns the serial numbers of all the attached probes to the specified GPIO Pin.
        ///// Useful for getting serial numbers to register each probe.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("{gpioPin}")]
        //public async Task<List<string>> GetSerialNumbers(int gpioPin)
        //{
        //    var serialNumbers = await _dS18B20Service.GetSerialNumbers(gpioPin);

        //    return serialNumbers;
        //}
    }
}
