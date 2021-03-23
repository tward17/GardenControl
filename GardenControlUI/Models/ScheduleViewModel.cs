using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlUI.Models
{
    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public GardenControlCore.Enums.TriggerType TriggerType { get; set; }
        public DateTime? TriggerTimeOfDay { get; set; }

        public int? TriggerOffsetAmount { get; set; }

        public GardenControlCore.Enums.TimeIntervalUnit? TriggerOffsetAmountTimeIntervalUnit { get; set; }

        public int? IntervalAmount { get; set; }

        public GardenControlCore.Enums.TimeIntervalUnit? IntervalAmountTimeIntervalUnit { get; set; }
        public string Trigger { get; set; }
        public int TaskCount { get; set; }
        public DateTime NextRunDateTime { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ScheduleTaskViewModel> ScheduleTasks { get; set; }
    }
}
