using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Helpers
{
    public static class EnumHelper
    {
        public static string GetControlDeviceTypeFriendlyName(Enums.DeviceType deviceType)
        {
            switch (deviceType)
            {
                case Enums.DeviceType.FloatSensor:
                    return "Float Sensor";
                default:
                    return Enum.GetName(typeof(Enums.DeviceType), deviceType);
            }
        }

        public static string GetTriggerTypeFriendlyName(Enums.TriggerType triggerType)
        {
            switch (triggerType)
            {
                case Enums.TriggerType.TimeOfDay:
                    return "Time of Day";
                default:
                    return Enum.GetName(typeof(Enums.TriggerType), triggerType);
            }
        }
    }
}
