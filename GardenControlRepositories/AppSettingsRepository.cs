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

        public async Task DeleteAppSettingAsync(int id)
        {
            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.AppSettingId == id).FirstOrDefaultAsync();

            if (appSettingEntity == null)
            {
                _logger.LogWarning($"Tried deleting AppSetting that does not exist, AppSettingId: {id}");
                return;
            }

            if (!appSettingEntity.CanBeUpdated)
                throw new InvalidOperationException($"Cannot delete App Setting with key: {appSettingEntity.Key}");

            try
            {
                _context.AppSettingEntities.Remove(appSettingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting AppSetting ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppSettingEntity>> GetAllAppSettingsAsync()
        {
            return await _context.AppSettingEntities.ToListAsync();
        }

        public async Task<AppSettingEntity> GetAppSettingByIdAsync(int id)
        {
            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.AppSettingId == id).FirstOrDefaultAsync();

            return appSettingEntity;
        }

        public async Task<AppSettingEntity> GetAppSettingByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.Key.ToUpper() == key.ToUpper()).FirstOrDefaultAsync();

            return appSettingEntity;
        }

        public async Task<AppSettingEntity> InsertAppSettingAsync(string key, string value, bool canBeUpdated = true)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            var appSettingEntity = new AppSettingEntity
            {
                Key = key,
                Value = value,
                CanBeUpdated = canBeUpdated
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

            return appSettingEntity;
        }

        public async Task<AppSettingEntity> UpdateAppSettingAsync(AppSettingEntity appSetting)
        {
            if (string.IsNullOrWhiteSpace(appSetting.Key)) throw new ArgumentNullException(nameof(appSetting.Key));
            if (string.IsNullOrWhiteSpace(appSetting.Value)) throw new ArgumentNullException(nameof(appSetting.Value));

            var appSettingEntity = await _context.AppSettingEntities
                .Where(x => x.AppSettingId == appSetting.AppSettingId).FirstOrDefaultAsync();

            if (appSettingEntity == null)
            {
                _logger.LogWarning($"Tried to update AppSetting for id that doesn't exist, id: {appSetting.AppSettingId}");
                throw new ArgumentException($"No AppSetting exists with id: {appSetting.AppSettingId}");
            }

            if (!appSettingEntity.CanBeUpdated)
                throw new InvalidOperationException($"Cannot delete App Setting with key: {appSettingEntity.Key}");

            appSettingEntity.Value = appSetting.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating AppSetting ({appSetting.AppSettingId}): {ex.Message}");
                throw;
            }

            return appSettingEntity;
        }
    }
}
