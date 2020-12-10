using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlRepositories
{
    public class AppSettingsRepository : IAppSettingsRepository
    {
        private GardenControlContext _context { get; init; }
        private ILogger<AppSettingsRepository> _logger { get; init; }

        public AppSettingsRepository(GardenControlContext context, ILogger<AppSettingsRepository> logger)
        {
            _context = context;
            _logger = logger;

            _context.Database.EnsureCreated();
        }

        public async Task DeleteSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.Key == key).FirstOrDefaultAsync();

            if (appSettingEntity == null)
            {
                _logger.LogInformation($"Tried deleting AppSetting that does not exist, key: {key}");
                return;
            }

            try
            {
                _context.AppSettingEntities.Remove(appSettingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting AppSetting ({key}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppSettingEntity>> GetAllSettings()
        {
            var appSettingEntity = await _context.AppSettingEntities.ToListAsync();

            return appSettingEntity;
        }

        public async Task<AppSettingEntity> GetSettingByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.Key.ToUpper() == key.ToUpper()).FirstOrDefaultAsync();

            return appSettingEntity;
        }

        public async Task InsertSetting(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            var appSettingEntity = new AppSettingEntity
            {
                Key = key,
                Value = value
            };

            try
            {
                _context.Add<AppSettingEntity>(appSettingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Inserting AppSetting ({key}, {value}): {ex.Message}");
                throw;
            }
        }

        public async Task UpdateSetting(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.Key == key).FirstOrDefaultAsync();

            if (appSettingEntity == null)
            {
                _logger.LogWarning($"Tried to update AppSetting for key that doesn't exist, Key: {key}");
                throw new ArgumentException($"No AppSetting exists with key: {key}");
            }

            appSettingEntity.Value = value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating AppSetting ({key}): {ex.Message}");
                throw;
            }
        }
    }
}
