using GardenControlCore.Enums;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("Schedule")]
    public class ScheduleEntity
    {
        [Key]
        public int ScheduleId { get; init; }
        
        [Required]
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        [Required]
        public TriggerType TriggerTypeId { get; set; }

        public DateTime? TriggerTimeOfDay { get; set; }

        public int? TriggerOffsetAmount { get; set; }

        public TimeIntervalUnit? TriggerOffsetAmountTimeIntervalUnitId { get; set; }

        public int? IntervalAmount { get; set; }

        public TimeIntervalUnit? IntervalAmountTimeIntervalUnitId { get; set; }

        [Required]
        public DateTime NextRunDateTime { get; set; }

        public virtual ICollection<ScheduleTaskEntity> ScheduleTasks { get; set; }
    }
}
