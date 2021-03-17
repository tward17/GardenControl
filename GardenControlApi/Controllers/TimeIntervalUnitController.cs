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
        /// <summary>
        /// Returns all the possible Time Interval Units
        /// </summary>
        /// <returns>List of Time Interval Units</returns>
        /// <response code="200">Returns all the possible Time Interval Units</response>
        [HttpGet(Name = "TimeIntervalUnitGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TimeIntervalUnitDto>))]
        public List<TimeIntervalUnitDto> Get()
        {
            var intervalunits = new List<TimeIntervalUnitDto>();

            foreach(var intervalUnitId in Enum.GetValues(typeof(TimeIntervalUnit)))
            {
                intervalunits.Add(new TimeIntervalUnitDto { Id = (int)intervalUnitId, Name = Enum.GetName(typeof(TimeIntervalUnit), intervalUnitId)});
            }

            return intervalunits;
        }

        /// <summary>
        /// Returns a single Time Interval Unit
        /// </summary>
        /// <returns>The speicied Time Interval Unit</returns>
        /// <response code="200">Returns the specified Time Interval Unit</response>
        /// <response code="404">Could not find Time Interval Unit with id</response>
        [HttpGet("{id}", Name = "TimeIntervalUnitGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeIntervalUnitDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TimeIntervalUnitDto> Get([FromRoute] int id)
        {
            if (Enum.IsDefined(typeof(TimeIntervalUnit), id))
            {
                var timeInterval = new TimeIntervalUnitDto { Id = id, Name = Enum.GetName(typeof(TimeIntervalUnit), id)};
                return timeInterval;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
