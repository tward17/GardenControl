using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Scheduler
{
    // Relay on
    // Relay off
    // Relay Toggle
    // Take DS18B20 Measurement
    // Check Float Sensor State
    public class TaskAction
    {
        public int TaskActionId { get; set; }
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
