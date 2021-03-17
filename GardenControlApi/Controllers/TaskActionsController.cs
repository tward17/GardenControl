using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
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
    public class TaskActionsController : Controller
    {
        private IScheduleService _scheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public TaskActionsController(IMapper mapper, IScheduleService scheduleService)
        {
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Returns all the possible Task Actions
        /// </summary>
        /// <returns>List of Task Actions</returns>
        /// <response code="200">Returns all the possible Task Actions</response>
        [HttpGet(Name = "TaskActionGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskActionDto>))]
        public async Task<IEnumerable<TaskActionDto>> Get()
        {
            return _mapper.Map<List<TaskActionDto>>(_scheduleService.GetTaskActions());
        }

        /// <summary>
        /// Returns a single Task Actions
        /// </summary>
        /// <returns>The speicied Task Actions</returns>
        /// <response code="200">Returns the specified Task Action</response>
        /// <response code="404">Could not find Task Action with id</response>
        [HttpGet("{id}", Name = "TaskActionGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskActionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskActionDto>> Get([FromRoute] TaskActionId id)
        {
            var taskAction = _mapper.Map<TaskActionDto>(_scheduleService.GetTaskActions().Where(ta => ta.TaskActionId == id).FirstOrDefault());

            if (taskAction == null)
                return NotFound();

            return taskAction;
        }
    }
}
