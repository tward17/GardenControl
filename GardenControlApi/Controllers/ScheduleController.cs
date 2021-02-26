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

namespace GardenControlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private IScheduleService _scheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public ScheduleController(IControlDeviceService controlDeviceService, IMapper mapper, IScheduleService scheduleService)
        {
            _controlDeviceService = controlDeviceService;
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Returns All Schedules
        /// </summary>
        /// <returns>Returns all Schedules</returns>
        /// <response code="200">Returns all Schedules</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScheduleDto>))]
        public async Task<List<ScheduleDto>> Get()
        {
            return _mapper.Map<List<ScheduleDto>>(await _scheduleService.GetAllSchedulesAsync());
        }

        /// <summary>
        /// Returns All Schedules
        /// </summary>
        /// <returns>Returns all Schedules</returns>
        /// <response code="200">Returns all Schedules</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScheduleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ScheduleDto>> Get([FromRoute] int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            return _mapper.Map<ScheduleDto>(await _scheduleService.GetScheduleAsync(id));
        }

        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <returns>Creates a new Schedule</returns>
        /// <response code="201">Schedule created successfully</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ScheduleDto>> Insert([FromBody] ScheduleDto scheduleDto)
        {
            var newSchedule = _mapper.Map<Schedule>(scheduleDto);

            newSchedule = await _scheduleService.InsertScheduleAsync(newSchedule);

            return CreatedAtAction(nameof(Get), new { id = newSchedule.ScheduleId }, _mapper.Map<ScheduleDto>(newSchedule));
        }

        /// <summary>
        /// Updates Schedule
        /// </summary>
        /// <returns>Updates Schedule</returns>
        /// <response code="204">Schedule updated successfully</response>
        /// <response code="400">Schedule id in url and object do not match</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ScheduleDto scheduleDto)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            if (id != scheduleDto.ScheduleId)
                return BadRequest();

            await _scheduleService.UpdateScheduleAsync(_mapper.Map<Schedule>(scheduleDto));

            return NoContent();
        }

        /// <summary>
        /// Deletes Schedule
        /// </summary>
        /// <returns>Deletes Schedule</returns>
        /// <response code="204">Schedule deleted successfully</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            await _scheduleService.DeleteScheduleAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Returns All Tasks for the specified Schedule
        /// </summary>
        /// <returns>Returns list of Schedule Tasks</returns>
        /// <response code="200">Returns Schedules Tasks for Schedule</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpGet("{id}/Tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScheduleTaskDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ScheduleTaskDto>>> GetTasks([FromRoute] int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            return _mapper.Map<List<ScheduleTaskDto>>(await _scheduleService.GetScheduleTasksAsync(id));
        }

        private async Task<bool> ScheduleExists(int id)
        {
            if (await _scheduleService.GetScheduleAsync(id) != null)
                return true;

            return false;
        }
    }
}
