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
        public string TriggerType { get; set; }
        public string Trigger { get; set; }
        public int TaskCount { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsActive { get; set; }
    }
}
