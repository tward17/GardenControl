using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    /// <summary>
    /// Responsible for CRUD operations for measurement records, eg recording temperatures
    /// </summary>
    public interface IMeasurementRepository
    {
        public Task<MeasurementEntity> InsertMeasurementAsync(MeasurementEntity measurement);
        public Task<MeasurementEntity> GetMeasurementByIdAsync(long id);
        public Task<List<MeasurementEntity>> GetAllMeasurementsAsync();
        public Task<List<MeasurementEntity>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<List<MeasurementEntity>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId);
        public Task<MeasurementEntity> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<MeasurementEntity> UpdateMeasurementAsync(MeasurementEntity measurement);
        public Task DeleteMeasurementAsync(long id);
    }
}
