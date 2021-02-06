using GardenControlCore.Enums;
using GardenControlCore.Models;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class FloatSensorService
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private ILogger<FloatSensorService> _logger { get; init; }

        public FloatSensorService(IControlDeviceService controlDeviceService, ILogger<FloatSensorService> logger)
        {
            _controlDeviceService = controlDeviceService;
            _logger = logger;
        }

        public async Task<FloatSensorState> GetFloatSensorState(int id)
        {
            var controlDevice = await _controlDeviceService.GetDevice(id);

            ValidateControlDevice(controlDevice);

            // Get the state of the pin. Need to check reliability of this, as reading the pin state can change it's value.
            var gpio = new GpioController();

            // Pull Up configuration - Sensor must be wired to GPIO and GND 
            gpio.OpenPin(controlDevice.GPIOPinNumber.Value, PinMode.InputPullUp);

            var state = gpio.Read(controlDevice.GPIOPinNumber.Value);

            if(state == PinValue.High)
            {
                return FloatSensorState.High;
            }
            else
            {
                return FloatSensorState.Low;
            }
        }

        private void ValidateControlDevice(ControlDevice controlDevice)
        {
            if (controlDevice == null)
                throw new Exception($"No device with id: {controlDevice.ControlDeviceId} found");
            else if (controlDevice.DeviceTypeId != GardenControlCore.Enums.DeviceType.FloatSensor)
                throw new Exception($"Specified Control Device with id: {controlDevice.ControlDeviceId} is not a Float Sensor");
            else if (!controlDevice.GPIOPinNumber.HasValue)
                throw new Exception($"Control Device with id: {controlDevice.ControlDeviceId} does not have a GPIO Pin specifed");
        }
    }
}
