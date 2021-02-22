using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Enums;
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
    public class MeasurementUnitController : Controller
    {
        private IMeasurementService _measurementService { get; init; }
        private IMapper _mapper { get; init; }

        public MeasurementUnitController(IMapper mapper, IMeasurementService measurementService)
        {
            _mapper = mapper;
            _measurementService = measurementService;
        }

        /// <summary>
        /// Returns All Measurement Units
        /// </summary>
        /// <returns>Returns all Measurement Units</returns>
        /// <response code="200">Returns all measurement units</response>
        /// <response code="404">Could not find Measurement from id</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeasurementUnitDto))]
        public async Task<List<MeasurementUnitDto>> Get()
        {
            return _mapper.Map<List<MeasurementUnitDto>>(_measurementService.GetAllMeasurementUnits());
        }

        /// <summary>
        /// Returns specified Measurement Unit
        /// </summary>
        /// <returns>Returns single Measurement Unit</returns>
        /// <response code="200">Returns specified measurement unit</response>
        /// <response code="404">Could not find Measurement Unit from id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeasurementUnitDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MeasurementUnitDto>> Get(int id)
        {
            var measurementUnit = _mapper.Map<MeasurementUnitDto>(_measurementService.GetMeasurementUnit(id));

            if (measurementUnit == null)
                return NotFound();

            return measurementUnit;
        }
    }
}
