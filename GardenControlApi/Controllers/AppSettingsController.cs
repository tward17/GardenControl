using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using GardenControlServices.Interfaces;
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
        [HttpGet("{id}")]
        public async Task<AppSetting> Get(int id)
        {
            var val = await _appSettingsService.GetAppSettingByIdAsync(id);

            return val;
        }

        /// <summary>
        /// Inserts setting
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A newly created AppSetting</returns>
        /// <reponse code="201">Returns the newly created AppSetting</reponse>
        /// <reponse code="400">If the key or value is null or empty</reponse>
        /// <remarks>Any value set for CanBeUpdated property will be ignored and always set to true</remarks>
        // POST api/<AppSettingsController>
        [HttpPost]
        public async Task<ActionResult<AppSetting>> Post([FromBody] AppSettingDto value)
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
        // PUT api/<AppSettingsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AppSettingDto appSettingDto)
        {
            if (id != appSettingDto.AppSettingId)
                return BadRequest();

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
        // DELETE api/<AppSettingsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appSettingsService.DeleteAppSettingAsync(id);

            return NoContent();
        }
    }
}
