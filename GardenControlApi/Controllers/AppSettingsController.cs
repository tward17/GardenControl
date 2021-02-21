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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GardenControlApi.Controllers
{
    /// <summary>
    /// End point for managing settings required for the application. Format of settings is Key/Value pair of String/String
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class AppSettingsController : ControllerBase
    {
        private IAppSettingsService _appSettingsService { get; init; }
        private IMapper _mapper { get; init; }

        public AppSettingsController(IMapper mapper, IAppSettingsService settingsService)
        {
            _mapper = mapper;
            _appSettingsService = settingsService;
        }

        /// <summary>
        /// Returns all settings
        /// </summary>
        /// <returns>All of the AppSettings</returns>
        /// <response code="200">Returns the all AppSettings</response>
        // GET: api/<AppSettingsController>
        [HttpGet]
        public async Task<IEnumerable<AppSetting>> Get()
        {
            return await _appSettingsService.GetAllSettingsAsync();
        }

        /// <summary>
        /// Returns specified setting
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The AppSetting for the provided key</returns>
        /// <response code="200">Returns the the specified AppSetting</response>
        /// <response code="404">If the AppSetting cannot be found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppSetting))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (!(await AppSettingExists(id)))
                return NotFound();

            var val = await _appSettingsService.GetAppSettingByIdAsync(id);

            return Ok(val);
        }

        /// <summary>
        /// Creates an AppSetting
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A newly created AppSetting</returns>
        /// <response code="201">Returns the newly created AppSetting</response>
        /// <response code="400">If the key or value is null or empty</response>
        /// <remarks>Any value set for CanBeUpdated property will be ignored and always set to true</remarks>
        // POST api/<AppSettingsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AppSetting))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AppSetting>> Insert([FromBody] AppSettingDto value)
        {
            if (string.IsNullOrWhiteSpace(value.Key))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(value.Value))
                throw new ArgumentNullException(nameof(value.Value));

            var newAppSetting = await _appSettingsService.InsertAppSettingAsync(value.Key, value.Value);

            return CreatedAtAction(nameof(Get), new { id = newAppSetting.AppSettingId }, newAppSetting);
        }

        /// <summary>
        /// Updates setting with specified value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appSettingDto"></param>
        /// <returns></returns>
        /// <response code="204">Returns when the AppSetting is updated sucessfully</response>
        /// <response code="400">If the id parameter and id in the object do not match</response>
        /// <response code="404">If the AppSetting cannot be found</response>
        // PUT api/<AppSettingsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, AppSettingDto appSettingDto)
        {
            if (id != appSettingDto.AppSettingId)
                return BadRequest();

            if (!(await AppSettingExists(id)))
                return NotFound();

            try
            {
                await _appSettingsService.UpdateAppSettingAsync(_mapper.Map<AppSetting>(appSettingDto));
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes specified setting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Returns when the AppSetting is deleted sucessfully</response>
        /// <response code="404">If the AppSetting cannot be found</response>
        // DELETE api/<AppSettingsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await AppSettingExists(id)))
                return NotFound();

            await _appSettingsService.DeleteAppSettingAsync(id);

            return NoContent();
        }

        private async Task<bool> AppSettingExists(int id)
        {
            if (await _appSettingsService.GetAppSettingByIdAsync(id) != null)
                return true;

            return false;
        }
    }
}
