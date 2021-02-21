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
    [Table("TaskSchedule")]
    public class TaskScheduleEntity
    {
        [Key]
        public int TaskScheduleId { get; init; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("TaskAction")]
        public int TaskActionId { get; set; }
        public virtual TaskAction TaskAction { get; set; }

        [Required]
        [ForeignKey("ControlDevice")]
        public int ControlDeviceId { get; set; }
        public virtual ControlDeviceEntity ControlDevice { get; set; }

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
    }
}
