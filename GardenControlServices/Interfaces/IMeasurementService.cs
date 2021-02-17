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
        public Task<IEnumerable<Measurement>> GetAllMeasurementsAsync();
        public Task<IEnumerable<Measurement>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<IEnumerable<Measurement>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId);
        public Task<Measurement> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId);
        public Task<Measurement> UpdateMeasurementAsync(Measurement measurement);
        public Task DeleteMeasurementAsync(long id);

        public List<MeasurementUnit> GetAllMeasurementUnits();
        public MeasurementUnit GetMeasurementUnit(int id);
    }
}
