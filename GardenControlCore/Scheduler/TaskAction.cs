using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Scheduler
{
    public class TaskAction
    {
        public TaskActionId TaskActionId { get; set; }
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
