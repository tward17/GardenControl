using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using GardenControlServices.Interfaces;
using Microsoft.AspNetCore.Http;
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
            return _mapper.Map<IEnumerable<ControlDeviceDto>>(await _deviceControlService.GetAllDevicesAsync());
        }

        // GET api/<ControlDeviceController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ControlDeviceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ControlDeviceDto>> Get(int id)
        {
            if (!(await ControlDeviceExists(id)))
                return NotFound();

            return _mapper.Map<ControlDeviceDto>(await _deviceControlService.GetDeviceAsync(id));
        }

        // POST api/<ControlDeviceController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] ControlDeviceDto value)
        {
            var newControlDevice = await _deviceControlService.InsertDeviceAsync(_mapper.Map<ControlDevice>(value));

            return CreatedAtAction(nameof(Get), new { id = newControlDevice.ControlDeviceId }, newControlDevice);
        }

        // PUT api/<ControlDeviceController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ControlDeviceDto value)
        {
            if (id != value.ControlDeviceId)
                return BadRequest();

            if (!(await ControlDeviceExists(id)))
                return NotFound();

            await _deviceControlService.UpdateDeviceAsync(_mapper.Map<ControlDevice>(value));

            return NoContent();
        }

        // DELETE api/<ControlDeviceController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await ControlDeviceExists(id)))
                return NotFound();

            await _deviceControlService.DeleteDeviceAsync(id);

            return NoContent();
        }

        private async Task<bool> ControlDeviceExists(int id)
        {
            if (await _deviceControlService.GetDeviceAsync(id) != null)
                return true;

            return false;
        }
    }
}
