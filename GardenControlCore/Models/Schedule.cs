using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class Schedule
    {
        public int ScheduleId { get; init; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public TriggerType TriggerType { get; set; }

        public DateTime? TriggerTimeOfDay { get; set; }

        public int? TriggerOffsetAmount { get; set; }

        public TimeIntervalUnit? TriggerOffsetAmountTimeIntervalUnit { get; set; }

        public int? IntervalAmount { get; set; }

        public TimeIntervalUnit? IntervalAmountTimeIntervalUnit { get; set; }

        public DateTime NextRunDateTime { get; set; }

        public ICollection<ScheduleTask> ScheduleTasks { get; set; }
    }
}
