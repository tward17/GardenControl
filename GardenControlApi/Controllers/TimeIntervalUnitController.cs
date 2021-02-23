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
    public class TimeIntervalUnitController : Controller
    {
        private ITaskScheduleService _taskScheduleService { get; init; }
        private IMapper _mapper { get; init; }

        public TimeIntervalUnitController(IMapper mapper, ITaskScheduleService taskScheduleService)
        {
            _mapper = mapper;
            _taskScheduleService = taskScheduleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeIntervalUnit))]
        public async Task<List<TimeIntervalUnit>> Get()
        {
            return _taskScheduleService.GetTaskActions();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeIntervalUnit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimeIntervalUnit>> Get(TaskActionId id)
        {
            var taskAction = _taskScheduleService.GetTaskActions().Where(ta => ta.TaskActionId == id).FirstOrDefault();

            if (taskAction == null)
                return NotFound();

            return taskAction;
        }
    }
}
