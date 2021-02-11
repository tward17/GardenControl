using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface IAppSettingsService
    {
        public Task<AppSetting> InsertAppSettingAsync(string key, string value);
        public Task<IEnumerable<AppSetting>> GetAllSettingsAsync();
        public Task<AppSetting> GetAppSettingByIdAsync(int id);
        public Task<AppSetting> GetAppSettingByKeyAsync(string key);
        public Task DeleteAppSettingAsync(int id);
        public Task<AppSetting> UpdateAppSettingAsync(AppSetting appSetting);
    }
}
