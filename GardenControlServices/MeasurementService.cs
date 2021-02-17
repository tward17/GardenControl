using AutoMapper;
using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class MeasurementService : IMeasurementService
    {
        private IMeasurementRepository _measurementRepository { get; init; }
        private ILogger<MeasurementService> _logger { get; init; }
        private IMapper _mapper { get; init; }

        private List<MeasurementUnit> measurementUnits { get; init; }

        public MeasurementService(IMeasurementRepository measurementRepository, ILogger<MeasurementService> logger, IMapper mapper)
        {
            _measurementRepository = measurementRepository;
            _logger = logger;
            _mapper = mapper;

            measurementUnits = new List<MeasurementUnit>
            {
                new MeasurementUnit{ MeasurementUnitId = 1, Name = "Celcius", Symbol = "C"},
                new MeasurementUnit{ MeasurementUnitId = 2, Name = "Fahrenheit", Symbol = "F" }
            };
        }

        public async Task DeleteMeasurementAsync(long id)
        {
            await _measurementRepository.DeleteMeasurementAsync(id);
        }

        public async Task<IEnumerable<Measurement>> GetAllMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            return _mapper.Map<IEnumerable<Measurement>>(await _measurementRepository.GetAllMeasurementByControlDeviceIdAsync(controlDeviceId));
        }

        public async Task<IEnumerable<Measurement>> GetAllMeasurementsAsync()
        {
            return _mapper.Map<IEnumerable<Measurement>>(await _measurementRepository.GetAllMeasurementsAsync());
        }

        public async Task<IEnumerable<Measurement>> GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(DateTime startDate, DateTime endDate, int controlDeviceId)
        {
            return _mapper.Map<IEnumerable<Measurement>>(await _measurementRepository.GetAllMeasurementsByControlDeviceIdAndDateRangeAsync(startDate, endDate, controlDeviceId));
        }

        public async Task<Measurement> GetLatestMeasurementByControlDeviceIdAsync(int controlDeviceId)
        {
            return _mapper.Map<Measurement>(await _measurementRepository.GetLatestMeasurementByControlDeviceIdAsync(controlDeviceId));
        }

        public async Task<Measurement> GetMeasurementByIdAsync(long id)
        {
            return _mapper.Map<Measurement>(await _measurementRepository.GetMeasurementByIdAsync(id));
        }

        public async Task<Measurement> InsertMeasurementAsync(Measurement measurement)
        {
            return _mapper.Map<Measurement>(await _measurementRepository.InsertMeasurementAsync(_mapper.Map<MeasurementEntity>(measurement)));
        }

        public async Task<Measurement> UpdateMeasurementAsync(Measurement measurement)
        {
            return _mapper.Map<Measurement>(await _measurementRepository.UpdateMeasurementAsync(_mapper.Map<MeasurementEntity>(measurement)));
        }

        public List<MeasurementUnit> GetAllMeasurementUnits()
        {
            return measurementUnits;
        }

        public MeasurementUnit GetMeasurementUnit(int id)
        {
            return measurementUnits.Where(x => x.MeasurementUnitId == id).FirstOrDefault();
        }
    }
}
