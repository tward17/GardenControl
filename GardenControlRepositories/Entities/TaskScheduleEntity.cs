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
        public virtual TaskActionEntity TaskAction { get; set; }

        [Required]
        [ForeignKey("ControlDevice")]
        public int ControlDeviceId { get; set; }
        public virtual ControlDeviceEntity ControlDevice { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("TriggerType")]
        public int TriggerTypeId { get; set; }
        public virtual TriggerTypeEntity TriggerType { get; set; }

        public DateTime? TriggerTimeOfDay { get; set; }

        public int? TriggerOffsetAmount { get; set; }

        [ForeignKey("TriggerOffsetAmountTimeInterval")]
        public int? TriggerOffsetAmountTimeIntervalId { get; set; }
        public TimeIntervalEntity TriggerOffsetAmountTimeInterval { get; set; }

        public int? IntervalAmount { get; set; }

        [ForeignKey("IntervalAmountTimeInterval")]
        public int? IntervalAmountTimeIntervalId { get; set; }
        public TimeIntervalEntity IntervalAmountTimeInterval { get; set; }

        [Required]
        public DateTime NextRunDateTime { get; set; }
    }
}
