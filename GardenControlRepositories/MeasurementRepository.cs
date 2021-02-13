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
    public class MeasurementRepository : IMeasurementRepository
    {
        private GardenControlContext _context { get; init; }
        private ILogger<MeasurementRepository> _logger { get; init; }

        public MeasurementRepository(GardenControlContext context, ILogger<MeasurementRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteMeasurementAsync(long id)
        {
            var measurementEntity = await _context.MeasurementEntities
                .Where(m => m.MeasurementId == id).FirstOrDefaultAsync();

            if (measurementEntity == null)
            {
                _logger.LogWarning($"Tried deleting Measurement that does not exist, MeasurementId: {id}");
                return;
            }

            try
            {
                _context.MeasurementEntities.Remove(measurementEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Measurement ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<List<MeasurementEntity>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            var measurementEntities = await _context.MeasurementEntities
                .Where(m => m.ControlDeviceId == controlDeviceId)
                .ToListAsync();

            return measurementEntities;
        }

        public async Task<List<MeasurementEntity>> GetAllMeasurementsAsync()
        {
            var measurementEntities = await _context.MeasurementEntities
                .ToListAsync();

            return measurementEntities;
        }

        public async Task<List<MeasurementEntity>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId)
        {
            var measurementEntities = await _context.MeasurementEntities
                .Where(m => m.ControlDeviceId == controlDeviceId && m.MeasurementDateTime >= startDate && m.MeasurementDateTime <= endDate)
                .ToListAsync();

            return measurementEntities;
        }

        public async Task<MeasurementEntity> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            var measurementEntity = await _context.MeasurementEntities
                .Where(m => m.ControlDeviceId == controlDeviceId)
                .OrderByDescending(m => m.MeasurementDateTime)
                .FirstOrDefaultAsync();

            return measurementEntity;
        }

        public async Task<MeasurementEntity> GetMeasurementByIdAsync(long id)
        {
            var measurementEntity = await _context.MeasurementEntities
                .Where(m => m.MeasurementId == id)
                .FirstOrDefaultAsync();

            return measurementEntity;
        }

        public async Task<MeasurementEntity> InsertMeasurementAsync(MeasurementEntity measurement)
        {
            if (measurement == null) throw new ArgumentNullException(nameof(measurement));

            try
            {
                _context.MeasurementEntities.Add(measurement);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting Measurement: {ex.Message}");
                throw;
            }

            return measurement;
        }

        public async Task<MeasurementEntity> UpdateMeasurementAsync(MeasurementEntity measurement)
        {
            var measurementEntity = await _context.MeasurementEntities
                .Where(m => m.MeasurementId == measurement.MeasurementId)
                .FirstOrDefaultAsync();

            if (measurementEntity == null)
                throw new Exception();

            measurementEntity.MeasurementValue = measurement.MeasurementValue;
            measurementEntity.MeasurementUnit = measurement.MeasurementUnit;
            measurementEntity.MeasurementDateTime = measurementEntity.MeasurementDateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO: What do to if update fails?
                _logger.LogError($"Error Updating Measurement ({measurement.MeasurementId}): {ex.Message}");
                throw;
            }

            return measurementEntity;
        }
    }
}
