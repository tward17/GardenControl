﻿using AutoMapper;
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
    public class ScheduleTaskController : Controller
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private IScheduleService _scheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public ScheduleTaskController(IControlDeviceService controlDeviceService, IMapper mapper, IScheduleService scheduleService)
        {
            _controlDeviceService = controlDeviceService;
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Returns All Schedule Tasks
        /// </summary>
        /// <returns>Returns all Schedule Tasks</returns>
        /// <response code="200">Returns all Schedule Tasks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScheduleTaskDto>))]
        public async Task<List<ScheduleTaskDto>> Get()
        {
            return _mapper.Map<List<ScheduleTaskDto>>(await _scheduleService.GetAllScheduleTasksAsync());
        }

        /// <summary>
        /// Returns specified Schedule Task
        /// </summary>
        /// <returns>Returns single Schedule Task</returns>
        /// <response code="200">Returns single Schedule Task</response>
        /// <response code="404">Could not find Schedule Task with given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScheduleTaskDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ScheduleTaskDto>> Get(int id)
        {
            if (!(await ScheduleTaskExists(id)))
                return NotFound();

            return _mapper.Map<ScheduleTaskDto>(await _scheduleService.GetScheduleTaskAsync(id));
        }

        /// <summary>
        /// Creates a new Schedule Task
        /// </summary>
        /// <returns>Created Schedule Task</returns>
        /// <response code="201">Schedule Task created successfully</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ScheduleTaskDto>> Insert(ScheduleTaskDto scheduleTaskDto)
        {
            var newScheduleTask = _mapper.Map<ScheduleTask>(scheduleTaskDto);

            newScheduleTask = await _scheduleService.InsertScheduleTaskAsync(newScheduleTask);
            
            return CreatedAtAction(nameof(Get), new { id = newScheduleTask.ScheduleTaskId }, _mapper.Map<ScheduleTaskDto>(newScheduleTask));
        }

        /// <summary>
        /// Updates Schedule Task
        /// </summary>
        /// <returns>Updates Schedule Task</returns>
        /// <response code="204">Schedule Task updated successfully</response>
        /// <response code="400">Schedule Task id in url and object do not match</response>
        /// <response code="404">Could not find Schedule Task with given id</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromQuery]int id, [FromBody]ScheduleTaskDto scheduleTaskDto)
        {
            if (!(await ScheduleTaskExists(id)))
                return NotFound();

            if (id != scheduleTaskDto.ScheduleTaskId)
                return BadRequest();

            await _scheduleService.UpdateScheduleTaskAsync(_mapper.Map<ScheduleTask>(scheduleTaskDto));

            return NoContent();
        }

        /// <summary>
        /// Deletes Schedule Task
        /// </summary>
        /// <returns>Deletes Schedule Task</returns>
        /// <response code="204">Schedule Task deleted successfully</response>
        /// <response code="404">Could not find Schedule Task with given id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await ScheduleTaskExists(id)))
                return NotFound();

            await _scheduleService.DeleteScheduleAsync(id);

            return NoContent();
        }

        private async Task<bool> ScheduleTaskExists(int id)
        {
            if (await _scheduleService.GetScheduleTaskAsync(id) != null)
                return true;

            return false;
        }
    }
}
