using GardenControlCore.Models;
using GardenControlServices.Interfaces;
using Iot.Device.OneWire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace GardenControlServices
{
    public class DS18B20Service
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private ILogger<DS18B20Service> _logger { get; init; }

        public DS18B20Service(IControlDeviceService controlDeviceService, ILogger<DS18B20Service> logger)
        {
            _controlDeviceService = controlDeviceService;
            _logger = logger;
        }

        public async Task<TemperatureReading> GetTemperatureReading(int id)
        {
            var controlDevice = await _controlDeviceService.GetDevice(id);

            if (controlDevice == null)
                throw new ArgumentException($"No device with id: {id} found");
            else if (controlDevice.DeviceTypeId != GardenControlCore.Enums.DeviceType.DS18B20)
                throw new ArgumentException($"Specified Control Device is not a DS18B20 Sensor");
            else if (!controlDevice.GPIOPinNumber.HasValue)
                throw new Exception($"Control Device with id: {id} does not have a GPIO Pin specifed");
            else if (string.IsNullOrWhiteSpace(controlDevice.SerialNumber))
                throw new Exception($"Control Device with id: { id } does not have a Serial Number specifed");

            TemperatureReading temperatureReading = null;

            foreach(var probe in OneWireThermometerDevice.EnumerateDevices())
            {
                var oneWireMeasurement = await probe.ReadTemperatureAsync();

                if (probe.DeviceId == controlDevice.SerialNumber)
                {
                    temperatureReading = new TemperatureReading
                    {
                        ReadingDateTime = DateTime.Now,
                        TemperatureC = oneWireMeasurement.DegreesCelsius,
                        TemperatureF = oneWireMeasurement.DegreesFahrenheit
                    };
                    break;
                }
            }

            return temperatureReading;
        }
    }
}
