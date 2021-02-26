using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Enums
{
    public enum TaskActionId {
        RelayOn = 1,
        RelayOff,
        RelayToggle,
        DS18B20Reading,
        FloatSensorStateReading
    }

    public enum TimeIntervalUnit
    {
        Seconds = 1,
        Minutes,
        Hours,
        Days
    }

    public enum TriggerType
    {
        TimeOfDay = 1,
        Interval,
        Sunrise,
        Sunset
    }
}
