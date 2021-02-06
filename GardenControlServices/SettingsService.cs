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
    public class SettingsService : ISettingsService
    {
        private ILogger<SettingsService> _logger { get; init; }
        private IAppSettingsRepository _settingsRepository { get; init; }
        private IMapper _mapper { get; init; }

        public SettingsService(ILogger<SettingsService> logger, IAppSettingsRepository settingsRepository, IMapper mapper)
        {
            _logger = logger;
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task DeleteSetting(string key)
        {
            await _settingsRepository.DeleteSetting(key);
        }

        public async Task<IEnumerable<AppSetting>> GetAllSettings()
        {
            var appSettings = await _settingsRepository.GetAllSettings();
            var settings = _mapper.Map<IEnumerable<AppSetting>>(appSettings);
            return settings;
        }

        public async Task<AppSetting> GetSettingByKey(string key)
        {
            var appSetting = await _settingsRepository.GetSetting(key);
            var setting = _mapper.Map<AppSetting>(appSetting);
            return setting;
        }

        public async Task InsertSetting(string key, string value)
        {
            var keyAlreadyExists = await _settingsRepository.GetSetting(key);

            if (keyAlreadyExists != null)
                throw new ArgumentException($"A setting with the key '{key}' already exists in the database");

            await _settingsRepository.InsertSetting(key, value);
        }

        public async Task UpdateSetting(string key, string value)
        {
            await _settingsRepository.UpdateSetting(key, value);
        }
    }
}
