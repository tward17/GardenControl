using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories
{
    public class ControlDeviceRepository : IControlDeviceRepository
    {
        private GardenControlContext _context { get; init; }
        private ILogger<ControlDeviceRepository> _logger { get; init; }
        
        public ControlDeviceRepository(GardenControlContext context, ILogger<ControlDeviceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteDevice(int id)
        {
            var deviceEntity = await _context.ControlDeviceEntities
                .Where(de => de.ControlDeviceId == id).FirstOrDefaultAsync();

            if (deviceEntity == null)
            {
                _logger.LogWarning($"Tried to delete ControlDevice using id that does not exist: ({id})");
                return;
            }

            try
            {
                _context.ControlDeviceEntities.Remove(deviceEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting ControlDevice ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ControlDeviceEntity>> GetAllDevices()
        {
            return await _context.ControlDeviceEntities.ToListAsync();
        }

        public async Task<ControlDeviceEntity> GetDevice(int id)
        {
            var deviceEntity = await _context.ControlDeviceEntities
                .Where(de => de.ControlDeviceId == id).FirstOrDefaultAsync();

            return deviceEntity;
        }

        public async Task InsertDevice(ControlDeviceEntity device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            try
            {
                _context.ControlDeviceEntities.Add(device);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting ControlDevice: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDevice(ControlDeviceEntity device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            var deviceEntity = await _context.ControlDeviceEntities
                .Where(de => de.ControlDeviceId == device.ControlDeviceId).FirstOrDefaultAsync();

            if (deviceEntity == null)
            {
                _logger.LogError($"Could not find ControlDevice to update. ControlDeviceId: {device.ControlDeviceId}");
                throw new ArgumentException(nameof(device));
            }

            deviceEntity.Alias = device.Alias;
            deviceEntity.Description = device.Description;
            deviceEntity.IsActive = device.IsActive;
            deviceEntity.GPIOPinNumber = device.GPIOPinNumber;
            deviceEntity.SerialNumber = device.SerialNumber;
            deviceEntity.DefaultState = device.DefaultState;

            try
            {
                _context.Update(deviceEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ControlDevice with ControlDeviceId: {deviceEntity.ControlDeviceId}. Error{ex.Message}");
                throw;
            }
        }
    }
}
