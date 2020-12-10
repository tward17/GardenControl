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
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private ISettingsService _appSettingsService { get; init; }

        public AppSettingsController(ISettingsService settingsService)
        {
            _appSettingsService = settingsService;
        }

        // GET: api/<AppSettingsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{key}")]
        public async Task<AppSetting> Get(string key)
        {
            var val = await _appSettingsService.GetSettingByKey(key);

            return val;
        }

        // POST api/<AppSettingsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AppSettingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AppSettingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
