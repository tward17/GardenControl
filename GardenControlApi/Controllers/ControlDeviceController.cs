using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using GardenControlServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GardenControlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlDeviceController : ControllerBase
    {
        private IControlDeviceService _deviceControlService { get; init; }
        private IMapper _mapper { get; init; }

        public ControlDeviceController(IControlDeviceService controlDeviceService, IMapper mapper)
        {
            _deviceControlService = controlDeviceService;
            _mapper = mapper;
        }

        // GET: api/<ControlDeviceController>
        [HttpGet]
        public async Task<IEnumerable<ControlDeviceDto>> Get()
        {
            return _mapper.Map<IEnumerable<ControlDeviceDto>>(await _deviceControlService.GetAllDevices());
        }

        // GET api/<ControlDeviceController>/5
        [HttpGet("{id}")]
        public async Task<ControlDeviceDto> Get(int id)
        {
            return _mapper.Map<ControlDeviceDto>(await _deviceControlService.GetDevice(id));
        }

        // POST api/<ControlDeviceController>
        [HttpPost]
        public async Task Post([FromBody] ControlDeviceInsertDto value)
        {
            await _deviceControlService.InsertDevice(_mapper.Map<ControlDevice>(value));
        }

        // PUT api/<ControlDeviceController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] ControlDevice value)
        {
            await _deviceControlService.UpdateDevice(value);
        }

        // DELETE api/<ControlDeviceController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _deviceControlService.DeleteDevice(id);
        }
    }
}
