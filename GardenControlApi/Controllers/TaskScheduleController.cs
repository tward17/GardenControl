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
    public class TaskScheduleController : Controller
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private ITaskScheduleService _taskScheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public TaskScheduleController(IControlDeviceService controlDeviceService, IMapper mapper, ITaskScheduleService taskScheduleService)
        {
            _controlDeviceService = controlDeviceService;
            _mapper = mapper;
            _taskScheduleService = taskScheduleService;
        }

        /// <summary>
        /// Returns All Task Schedules
        /// </summary>
        /// <returns>Returns all Task Schedules</returns>
        /// <response code="200">Returns all Task Schedules</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskScheduleDto>))]
        public async Task<List<TaskScheduleDto>> Get()
        {
            return _mapper.Map<List<TaskScheduleDto>>(await _taskScheduleService.GetAllTaskSchedulesAsync());
        }

        /// <summary>
        /// Returns All Task Schedules
        /// </summary>
        /// <returns>Returns all Task Schedules</returns>
        /// <response code="200">Returns all Task Schedules</response>
        /// <response code="404">Could not find Task Schedule with given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskScheduleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskScheduleDto>> Get(int id)
        {
            if (!(await TaskScheduleExists(id)))
                return NotFound();

            return _mapper.Map<TaskScheduleDto>(await _taskScheduleService.GetTaskScheduleAsync(id));
        }

        /// <summary>
        /// Creates a new Task Schedule
        /// </summary>
        /// <returns>Creates a new Task Schedule</returns>
        /// <response code="201">Task Schedule created successfully</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskScheduleDto>> Insert(TaskScheduleDto taskScheduleDto)
        {
            var newTaskSchedule = _mapper.Map<TaskSchedule>(taskScheduleDto);

            // newTaskSchedule.ControlDevice = new ControlDevice { ControlDeviceId = taskScheduleDto.ControlDeviceId };

            // get the task action to add to the object
            newTaskSchedule.TaskAction = _taskScheduleService.GetTaskActions().Where(ta => ta.TaskActionId == taskScheduleDto.TaskActionId).FirstOrDefault();

            newTaskSchedule = await _taskScheduleService.InsertTaskScheduleAsync(newTaskSchedule);
            
            return CreatedAtAction(nameof(Get), new { id = newTaskSchedule.TaskScheduleId }, _mapper.Map<TaskScheduleDto>(newTaskSchedule));
        }

        /// <summary>
        /// Updates Task Schedule
        /// </summary>
        /// <returns>Updates Task Schedule</returns>
        /// <response code="204">Task Schedule updated successfully</response>
        /// <response code="400">Task Schedule id in url and object do not match</response>
        /// <response code="404">Could not find Task Schedule with given id</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, TaskScheduleDto taskScheduleDto)
        {
            if (!(await TaskScheduleExists(id)))
                return NotFound();

            if (id != taskScheduleDto.TaskScheduleId)
                return BadRequest();

            await _taskScheduleService.UpdateTaskScheduleAsync(_mapper.Map<TaskSchedule>(taskScheduleDto));

            return NoContent();
        }

        /// <summary>
        /// Deletes Task Schedule
        /// </summary>
        /// <returns>Deletes Task Schedule</returns>
        /// <response code="204">Task Schedule deleted successfully</response>
        /// <response code="404">Could not find Task Schedule with given id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await TaskScheduleExists(id)))
                return NotFound();

            await _taskScheduleService.DeleteTaskScheduleAsync(id);

            return NoContent();
        }

        private async Task<bool> TaskScheduleExists(int id)
        {
            if (await _taskScheduleService.GetTaskScheduleAsync(id) != null)
                return true;

            return false;
        }
    }
}
