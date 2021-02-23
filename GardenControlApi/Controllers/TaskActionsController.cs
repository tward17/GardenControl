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
        private ITaskScheduleService _taskScheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public TaskActionsController(IMapper mapper, ITaskScheduleService taskScheduleService)
        {
            _mapper = mapper;
            _taskScheduleService = taskScheduleService;
        }

        /// <summary>
        /// Returns all the possible Task Actions
        /// </summary>
        /// <returns>List of Task Actions</returns>
        /// <response code="200">Returns all the possible Task Actions</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskAction>))]
        public async Task<List<TaskAction>> Get()
        {
            return _taskScheduleService.GetTaskActions();
        }

        /// <summary>
        /// Returns a single Task Actions
        /// </summary>
        /// <returns>The speicied Task Actions</returns>
        /// <response code="200">Returns the specified Task Action</response>
        /// <response code="404">Could not find Task Action with id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskAction))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskAction>> Get(TaskActionId id)
        {
            var taskAction = _taskScheduleService.GetTaskActions().Where(ta => ta.TaskActionId == id).FirstOrDefault();

            if (taskAction == null)
                return NotFound();

            return taskAction;
        }
    }
}
