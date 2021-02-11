using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    /// <summary>
    /// Performs CRUD operations for config settings
    /// </summary>
    public interface IAppSettingsRepository
    {
        public Task<AppSettingEntity> InsertAppSettingAsync(string key, string value, bool canBeUpdated = true);
        public Task<IEnumerable<AppSettingEntity>> GetAllAppSettingsAsync();
        public Task<AppSettingEntity> GetAppSettingByIdAsync(int id);
        public Task<AppSettingEntity> GetAppSettingByKeyAsync(string key);
        public Task DeleteAppSettingAsync(int id);
        public Task<AppSettingEntity> UpdateAppSettingAsync(AppSettingEntity appSetting);
    }
}
