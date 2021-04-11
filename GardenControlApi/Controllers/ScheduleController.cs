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
        [HttpGet(Name = "ScheduleGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Schedule>))]
        public async Task<IEnumerable<Schedule>> Get()
        {
            return await _scheduleService.GetAllSchedulesAsync();
        }

        /// <summary>
        /// Returns specified Schedule
        /// </summary>
        /// <returns>Returns specified Schedule</returns>
        /// <response code="200">Returns specified Schedule</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpGet("{id}", Name = "ScheduleGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Schedule))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Schedule>> Get([FromRoute] int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            return await _scheduleService.GetScheduleAsync(id);
        }

        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <returns>Creates a new Schedule</returns>
        /// <response code="201">Schedule created successfully</response>
        /// <response code="400"></response>
        [HttpPost(Name = "ScheduleInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Schedule>> Insert([FromBody] Schedule schedule)
        {
            var newSchedule = await _scheduleService.InsertScheduleAsync(schedule);

            return CreatedAtAction(nameof(Get), new { id = newSchedule.ScheduleId }, _mapper.Map<Schedule>(newSchedule));
        }

        /// <summary>
        /// Updates Schedule
        /// </summary>
        /// <returns>Updates Schedule</returns>
        /// <response code="204">Schedule updated successfully</response>
        /// <response code="400">Schedule id in url and object do not match</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpPut("{id}", Name = "ScheduleUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Schedule schedule)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            if (id != schedule.ScheduleId)
                return BadRequest();

            await _scheduleService.UpdateScheduleAsync(schedule);

            return NoContent();
        }

        /// <summary>
        /// Deletes Schedule
        /// </summary>
        /// <returns>Deletes Schedule</returns>
        /// <response code="204">Schedule deleted successfully</response>
        /// <response code="404">Could not find Schedule with given id</response>
        [HttpDelete("{id}", Name = "ScheduleDelete")]
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
        [HttpGet("{id}/Tasks", Name = "ScheduleTasksGetByScheduleId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScheduleTask>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ScheduleTask>>> GetTasks([FromRoute] int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            var scheduleTasks = await _scheduleService.GetScheduleTasksAsync(id);
            return scheduleTasks.ToList();
        }

        [HttpGet("{id}/Run", Name = "ScheduleRunNowById")]
        public async Task<ActionResult> RunScheduleNowById(int id)
        {
            if (!(await ScheduleExists(id)))
                return NotFound();

            var schedule = await _scheduleService.GetScheduleAsync(id);
            try
            {
                await _scheduleService.RunSchedule(schedule);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }

        [HttpGet("RunPendingSchedules", Name = "SchedulesRunPending")]
        public async Task<ActionResult> RunPendingSchdedules()
        {
            try
            {
                await _scheduleService.RunPendingSchedules();
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }

        private async Task<bool> ScheduleExists(int id)
        {
            if (await _scheduleService.GetScheduleAsync(id) != null)
                return true;

            return false;
        }
    }
}
