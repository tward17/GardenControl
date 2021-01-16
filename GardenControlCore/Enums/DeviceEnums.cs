using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Enums
{
    public enum DeviceType
    {
        Relay = 1,
        DS18B20 = 2,
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
