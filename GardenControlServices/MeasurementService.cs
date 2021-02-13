using GardenControlCore.Models;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class MeasurementService : IMeasurementService
    {
        private IMeasurementRepository _measurementRepository { get; init; }
        private ILogger<MeasurementService> _logger { get; init; }

        public MeasurementService(IMeasurementRepository measurementRepository, ILogger<MeasurementService> logger)
        {
            _measurementRepository = measurementRepository;
            _logger = logger;
        }

        public async Task DeleteMeasurementAsync(long id)
        {
            await _measurementRepository.DeleteMeasurementAsync(id);
        }

        public async Task<List<Measurement>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Measurement>> GetAllMeasurementsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Measurement>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<Measurement> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<Measurement> GetMeasurementByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Measurement> InsertMeasurementAsync(Measurement measurement)
        {
            throw new NotImplementedException();
        }

        public async Task<Measurement> UpdateMeasurementAsync(Measurement measurement)
        {
            throw new NotImplementedException();
        }
    }
}
