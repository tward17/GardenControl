using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Enums
{
    public enum TaskActionId {
        RelayOn,
        RelayOff,
        RelayToggle,
        DS18B20Reading,
        FloatSensorStateReading
    }

    public enum TimeIntervalUnit
    {
        Seconds,
        Minutes,
        Hours,
        Days
    }

    public enum TriggerType
    {
        TimeOfDay,
        Interval,
        Sunrise,
        Sunset
    }
}
