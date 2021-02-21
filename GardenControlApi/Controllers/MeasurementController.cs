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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MeasurementDto>))]
        public async Task<List<MeasurementDto>> Get()
        {
            return _mapper.Map<List<MeasurementDto>>(await _measurementService.GetAllMeasurementsAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeasurementDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MeasurementDto>> Get(long id)
        {
            if (!(await MeasurementExists(id)))
                return NotFound();

            return _mapper.Map<MeasurementDto>(await _measurementService.GetMeasurementByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MeasurementDto>> Insert(MeasurementDto measurementDto)
        {
            var newMeasurement = await _measurementService.InsertMeasurementAsync(_mapper.Map<Measurement>(measurementDto));
            return CreatedAtAction(nameof(Get), new { id = newMeasurement.MeasurementId }, newMeasurement);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, MeasurementDto measurementDto)
        {
            if (!(await MeasurementExists(id)))
                return NotFound();

            if (id != measurementDto.MeasurementId)
                return BadRequest();

            await _measurementService.UpdateMeasurementAsync(_mapper.Map<Measurement>(measurementDto));

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
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
