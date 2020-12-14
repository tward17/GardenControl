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
        private ISettingsService _appSettingsService { get; init; }

        public AppSettingsController(ISettingsService settingsService)
        {
            _appSettingsService = settingsService;
        }

        /// <summary>
        /// Returns all settings
        /// </summary>
        /// <returns></returns>
        // GET: api/<AppSettingsController>
        [HttpGet]
        public async Task<IEnumerable<AppSetting>> Get()
        {
            return await _appSettingsService.GetAllSettings();
        }

        /// <summary>
        /// Returns specified setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public async Task<AppSetting> Get(string key)
        {
            var val = await _appSettingsService.GetSettingByKey(key);

            return val;
        }

        /// <summary>
        /// Inserts setting
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST api/<AppSettingsController>
        [HttpPost]
        public async Task Post([FromBody] AppSetting value)
        {
            if (string.IsNullOrWhiteSpace(value.Key))
                throw new ArgumentNullException(nameof(value.Key));

            if (string.IsNullOrWhiteSpace(value.Value))
                throw new ArgumentNullException(nameof(value.Value));

            await _appSettingsService.InsertSetting(value.Key, value.Value);
        }

        /// <summary>
        /// Updates setting with specified value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/<AppSettingsController>/5
        [HttpPut("{key}")]
        public async Task Put(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            await _appSettingsService.UpdateSetting(key, value);
        }

        /// <summary>
        /// Deletes specified setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        // DELETE api/<AppSettingsController>/5
        [HttpDelete("{key}")]
        public async Task Delete(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            await _appSettingsService.DeleteSetting(key);
        }
    }
}
