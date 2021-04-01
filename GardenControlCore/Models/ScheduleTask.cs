using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class ScheduleTask
    {
        public int ScheduleTaskId { get; init; }

        public int ScheduleId { get; set; }

        public TaskAction TaskAction { get; set; }

        public ControlDevice ControlDevice { get; set; }

        public bool IsActive { get; set; }

    }
}
