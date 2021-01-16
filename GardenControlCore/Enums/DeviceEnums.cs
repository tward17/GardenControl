using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Enums
{
    public enum DeviceType
    {
        Relay = 1,
        [Display(Name = "DS18B20 Thermometer")]
        DS18B20 = 2,
        [Display(Name = "Float Sensor")]
        FloatSensor = 3
    }

    public enum DefaultState
    {
        Off = 0,
        On = 1
    }

    public enum RelayState
    {
        Off,
        On
    }

    public enum FloatSensorState
    {
        Low,
        High
    }
}
