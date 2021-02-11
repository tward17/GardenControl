using AutoMapper;
using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class AppSettingsService : IAppSettingsService
    {
        private ILogger<AppSettingsService> _logger { get; init; }
        private IAppSettingsRepository _settingsRepository { get; init; }
        private IMapper _mapper { get; init; }

        public AppSettingsService(ILogger<AppSettingsService> logger, IAppSettingsRepository settingsRepository, IMapper mapper)
        {
            _logger = logger;
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task DeleteAppSettingAsync(int id)
        {
            await _settingsRepository.DeleteAppSettingAsync(id);
        }

        public async Task<IEnumerable<AppSetting>> GetAllSettingsAsync()
        {
            var appSettings = await _settingsRepository.GetAllAppSettingsAsync();
            var settings = _mapper.Map<IEnumerable<AppSetting>>(appSettings);
            return settings;
        }

        public async Task<AppSetting> GetAppSettingByIdAsync(int id)
        {
            AppSetting setting = null;

            var appSetting = await _settingsRepository.GetAppSettingByIdAsync(id);
            
            if (appSetting != null)
                setting = _mapper.Map<AppSetting>(appSetting);

            return setting;
        }

        public async Task<AppSetting> GetAppSettingByKeyAsync(string key)
        {
            AppSetting setting = null;

            var appSetting = await _settingsRepository.GetAppSettingByKeyAsync(key);

            if (appSetting != null)
                setting = _mapper.Map<AppSetting>(appSetting);

            return setting;
        }

        public async Task<AppSetting> InsertAppSettingAsync(string key, string value)
        {
            var keyAlreadyExists = await _settingsRepository.GetAppSettingByKeyAsync(key);

            if (keyAlreadyExists != null)
                throw new ArgumentException($"A setting with the key '{key}' already exists in the database");

            var appSetting = _mapper.Map<AppSetting>(await _settingsRepository.InsertAppSettingAsync(key, value));

            return appSetting;
        }

        public async Task<AppSetting> UpdateAppSettingAsync(AppSetting appSetting)
        {
            var appSettingEntity = new AppSettingEntity
            {
                AppSettingId = appSetting.AppSettingId,
                Key = appSetting.Key,
                Value = appSetting.Value
            };

            var updatedAppSetting = _mapper.Map<AppSetting>(await _settingsRepository.UpdateAppSettingAsync(appSettingEntity));

            return updatedAppSetting;
        }       
    }
}
