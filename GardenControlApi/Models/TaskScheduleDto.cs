using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class TaskScheduleDto
    {
        public int TaskScheduleId { get; set; }
        public string Name { get; set; }

        public TaskActionId TaskActionId { get; set; }

        public int ControlDeviceId { get; set; }

        public bool IsActive { get; set; }

        public TriggerType TriggerTypeId { get; set; }

        public DateTime? TriggerTimeOfDay { get; set; }

        public int? TriggerOffsetAmount { get; set; }

        public TimeIntervalUnit? TriggerOffsetAmountTimeIntervalUnitId { get; set; }

        public int? IntervalAmount { get; set; }

        public TimeIntervalUnit? IntervalAmountTimeIntervalUnitId { get; set; }

        public DateTime NextRunDateTime { get; set; }
    }
}
