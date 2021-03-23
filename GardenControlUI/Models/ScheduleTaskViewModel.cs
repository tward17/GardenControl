using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlUI.Models
{
    public class ScheduleTaskViewModel
    {
        public ControlDeviceViewModel ControlDevice { get; set; }
        public int TaskAction { get; set; }
        public bool IsActive { get; set; }
    }
}
