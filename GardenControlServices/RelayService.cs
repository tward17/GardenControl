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
    public class RelayService
    {
        private IControlDeviceService _controlDeviceService { get; init; }
        private ILogger<RelayService> _logger { get; init; }

        public RelayService(IControlDeviceService controlDeviceService, ILogger<RelayService> logger)
        {
            _controlDeviceService = controlDeviceService;
            _logger = logger;
        }

        public async Task<RelayState> GetRelayState(int id)
        {
            var controlDevice = await _controlDeviceService.GetDeviceAsync(id);

            ValidateControlDevice(controlDevice);

            // Get the state of the pin. Need to check reliability of this, as reading the pin state can change it's value.
            var gpio = new GpioController();

            gpio.OpenPin(controlDevice.GPIOPinNumber.Value, PinMode.Output);

            var state = gpio.Read(controlDevice.GPIOPinNumber.Value);

            if (state == PinValue.High)
            {
                return RelayState.Off;
            }
            else
            {
                return RelayState.On;
            }
        }

        public async Task SetRelayState(int id, RelayState state)
        {
            var controlDevice = await _controlDeviceService.GetDeviceAsync(id);

            ValidateControlDevice(controlDevice);

            // Set the state of the pin. May need to cache to value
            var gpio = new GpioController();

            gpio.OpenPin(controlDevice.GPIOPinNumber.Value, PinMode.Output);

            if (state == RelayState.On)
            {
                gpio.Write(controlDevice.GPIOPinNumber.Value, PinValue.Low);
            }
            else
            {
                gpio.Write(controlDevice.GPIOPinNumber.Value, PinValue.High);
            }
        }

        public async Task ToggleRelayState(int id)
        {
            var controlDevice = await _controlDeviceService.GetDeviceAsync(id);

            ValidateControlDevice(controlDevice);

            // Toggle the state of the pin. May need to check the cache to get the current value instead of polling the pin.
        }

        private void ValidateControlDevice(ControlDevice controlDevice)
        {
            if (controlDevice == null)
                throw new Exception($"No device with id: {controlDevice.ControlDeviceId} found");
            else if (controlDevice.DeviceTypeId != GardenControlCore.Enums.DeviceType.Relay)
                throw new Exception($"Specified Control Device with id: {controlDevice.ControlDeviceId} is not a Relay");
            else if (!controlDevice.GPIOPinNumber.HasValue)
                throw new Exception($"Control Device with id: {controlDevice.ControlDeviceId} does not have a GPIO Pin specifed");
        }
    }
}
