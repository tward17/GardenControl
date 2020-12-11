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
        public Task InsertSetting(string key, string value);
        public Task<IEnumerable<AppSettingEntity>> GetAllSettings();
        public Task<AppSettingEntity> GetSetting(string key);
        public Task DeleteSetting(string key);
        public Task UpdateSetting(string key, string value);
    }
}
