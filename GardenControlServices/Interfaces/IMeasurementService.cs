using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface IMeasurementService
    {
        public Task<Measurement> InsertMeasurementAsync(Measurement measurement);
        public Task<Measurement> GetMeasurementByIdAsync(long id);
        public Task<List<Measurement>> GetAllMeasurementsAsync();
        public Task<List<Measurement>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<List<Measurement>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId);
        public Task<Measurement> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<Measurement> UpdateMeasurementAsync(Measurement measurement);
        public Task DeleteMeasurementAsync(long id);
    }
}
