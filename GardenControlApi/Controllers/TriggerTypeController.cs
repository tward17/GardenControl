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
    public class TriggerTypeController : Controller
    {
        /// <summary>
        /// Returns all the possible Trigger Types
        /// </summary>
        /// <returns>List of Trigger Types</returns>
        /// <response code="200">Returns all the possible Trigger Types</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TriggerTypeDto>))]
        public List<TriggerTypeDto> Get()
        {
            var triggerTypes = new List<TriggerTypeDto>();

            foreach(var triggerId in Enum.GetValues(typeof(TriggerType)))
            {
                triggerTypes.Add(new TriggerTypeDto { Id = (int)triggerId, Name = Enum.GetName(typeof(TriggerType), triggerId)});
            }

            return triggerTypes;
        }

        /// <summary>
        /// Returns a single Trigger Type
        /// </summary>
        /// <returns>The speicied Trigger Type</returns>
        /// <response code="200">Returns the specified Trigger Type</response>
        /// <response code="404">Could not find Trigger Type with id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TriggerTypeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TriggerTypeDto> Get(int id)
        {
            if (Enum.IsDefined(typeof(TriggerType), id))
            {
                var triggerType = new TriggerTypeDto { Id = id, Name = Enum.GetName(typeof(TriggerType), id) };
                return triggerType;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
