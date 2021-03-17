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

        /// <summary>
        /// Returns all Control Devices
        /// </summary>
        /// <returns>Returns all Control Devices</returns>
        /// <response code="200">Returns all control devices</response>
        [HttpGet(Name = "ControlDeviceGetAll")]
        public async Task<IEnumerable<ControlDeviceDto>> Get()
        {
            return _mapper.Map<IEnumerable<ControlDeviceDto>>(await _deviceControlService.GetAllDevicesAsync());
        }

        /// <summary>
        /// Returns a Control Device
        /// </summary>
        /// <returns>Returns the specified Control Device</returns>
        /// <response code="200">Returns the specified control device</response>
        /// <response code="404">Could not find a control device with the specified id</response>
        [HttpGet("{id}", Name = "ControlDeviceGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ControlDeviceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ControlDeviceDto>> Get([FromRoute] int id)
        {
            if (!(await ControlDeviceExists(id)))
                return NotFound();

            return _mapper.Map<ControlDeviceDto>(await _deviceControlService.GetDeviceAsync(id));
        }

        /// <summary>
        /// Creates a Control Device
        /// </summary>
        /// <returns>Returns the created Control Device</returns>
        /// <response code="201">Control Device created successfully</response>
        [HttpPost(Name = "ControlDeviceInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Insert([FromBody] ControlDeviceDto value)
        {
            var newControlDevice = await _deviceControlService.InsertDeviceAsync(_mapper.Map<ControlDevice>(value));

            return CreatedAtAction(nameof(Get), new { id = newControlDevice.ControlDeviceId }, newControlDevice);
        }

        /// <summary>
        /// Updates a Control Device
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Control Device successfully updated</response>
        /// <response code="400">Control Device Id in url and object do not match</response>
        /// <response code="404">Could not find a control device with the specified id</response>
        [HttpPut("{id}", Name = "ControlDeviceUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ControlDeviceDto value)
        {
            if (id != value.ControlDeviceId)
                return BadRequest();

            if (!(await ControlDeviceExists(id)))
                return NotFound();

            await _deviceControlService.UpdateDeviceAsync(_mapper.Map<ControlDevice>(value));

            return NoContent();
        }

        /// <summary>
        /// Deletes a Control Device
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Control Device successfully deleted</response>
        /// <response code="404">Could not find a control device with the specified id</response>
        [HttpDelete("{id}", Name = "ControlDeviceDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
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
