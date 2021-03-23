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
    public class MeasurementController : Controller
    {
        private IMeasurementService _measurementService { get; init; }
        private IMapper _mapper { get; init; }

        public MeasurementController(IMapper mapper, IMeasurementService measurementService)
        {
            _mapper = mapper;
            _measurementService = measurementService;
        }

        /// <summary>
        /// Returns all Measurements
        /// </summary>
        /// <returns>Collection of Measurements</returns>
        /// <response code="200">Returns all measurements</response>
        [HttpGet(Name = "MeasurementGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Measurement>))]
        public async Task<IEnumerable<Measurement>> Get()
        {
            return await _measurementService.GetAllMeasurementsAsync();
        }

        /// <summary>
        /// Returns specified Measurement
        /// </summary>
        /// <returns>Returns single Measurement</returns>
        /// <response code="200">Returns all measurements</response>
        /// <response code="404">Could not find Measurement from id</response>
        [HttpGet("{id}", Name = "MeasurementGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Measurement))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Measurement>> Get([FromRoute] long id)
        {
            if (!(await MeasurementExists(id)))
                return NotFound();

            return await _measurementService.GetMeasurementByIdAsync(id);
        }

        /// <summary>
        /// Creates a Measurement
        /// </summary>
        /// <returns>Returns created Measurement</returns>
        /// <response code="201">Measurement created successfully</response>
        /// <response code="400">Could not find Measurement from id</response>
        [HttpPost(Name = "MeasurementInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Measurement>> Insert([FromBody] Measurement measurement)
        {
            var newMeasurement = await _measurementService.InsertMeasurementAsync(measurement);
            return CreatedAtAction(nameof(Get), new { id = newMeasurement.MeasurementId }, newMeasurement);
        }

        /// <summary>
        /// Updates specified Measurement
        /// </summary>
        /// <returns>Updates specified Measurement</returns>
        /// <response code="204">Measurement updated successfully</response>
        /// <response code="400">Measurement Id in url and object do not match</response>
        /// <response code="404">Could not find Measurement from id</response>
        [HttpPut("{id}", Name = "MeasurementUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] Measurement measurement)
        {
            if (!(await MeasurementExists(id)))
                return NotFound();

            if (id != measurement.MeasurementId)
                return BadRequest();

            await _measurementService.UpdateMeasurementAsync(measurement);

            return NoContent();
        }

        /// <summary>
        /// Deletes specified Measurement
        /// </summary>
        /// <returns>Deletes single Measurement</returns>
        /// <response code="204">Measurement successfully deleted</response>
        /// <response code="404">Could not find Measurement from id</response>
        [HttpDelete("{id}", Name = "MeasurementDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!(await MeasurementExists(id)))
                return NotFound();

            await _measurementService.DeleteMeasurementAsync(id);

            return NoContent();
        }

        private async Task<bool> MeasurementExists(long id)
        {
            if (await _measurementService.GetMeasurementByIdAsync(id) != null)
                return true;

            return false;
        }
    }
}
