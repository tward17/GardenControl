using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ScheduleTaskDto
    {
        public int ScheduleTaskId { get; set; }

        public int ScheduleId { get; set; }

        public TaskActionId TaskActionId { get; set; }

        public int ControlDeviceId { get; set; }

        public bool IsActive { get; set; }
    }
}
