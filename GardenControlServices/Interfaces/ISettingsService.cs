using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface ISettingsService
    {
        public Task InsertSetting(string key, string value);
        public Task<IEnumerable<AppSetting>> GetAllSettings();
        public Task<AppSetting> GetSettingByKey(string key);
        public Task DeleteSetting(string key);
        public Task UpdateSetting(string key, string value);
    }
}
