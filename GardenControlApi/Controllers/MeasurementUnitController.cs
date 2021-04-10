using AutoMapper;
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
    //[Route("api/[controller]")]
    //[ApiController]
    //public class MeasurementUnitController : Controller
    //{
    //    private IMeasurementService _measurementService { get; init; }
    //    private IMapper _mapper { get; init; }

    //    public MeasurementUnitController(IMapper mapper, IMeasurementService measurementService)
    //    {
    //        _mapper = mapper;
    //        _measurementService = measurementService;
    //    }

    //    /// <summary>
    //    /// Returns All Measurement Units
    //    /// </summary>
    //    /// <returns>Returns all Measurement Units</returns>
    //    /// <response code="200">Returns all measurement units</response>
    //    /// <response code="404">Could not find Measurement from id</response>
    //    [HttpGet(Name = "MeasurementUnitGetAll")]
    //    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MeasurementUnit>))]
    //    public async Task<List<MeasurementUnit>> Get()
    //    {
    //        return _measurementService.GetAllMeasurementUnits();
    //    }

    //    /// <summary>
    //    /// Returns specified Measurement Unit
    //    /// </summary>
    //    /// <returns>Returns single Measurement Unit</returns>
    //    /// <response code="200">Returns specified measurement unit</response>
    //    /// <response code="404">Could not find Measurement Unit from id</response>
    //    [HttpGet("{id}", Name = "MeasurementUnitGetById")]
    //    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeasurementUnit))]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<ActionResult<MeasurementUnit>> Get([FromRoute] int id)
    //    {
    //        var measurementUnit = _measurementService.GetMeasurementUnit(id);

    //        if (measurementUnit == null)
    //            return NotFound();

    //        return measurementUnit;
    //    }
    //}
}
