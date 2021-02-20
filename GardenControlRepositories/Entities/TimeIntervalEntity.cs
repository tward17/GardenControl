using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("TimeInterval")]
    public class TimeIntervalEntity
    {
        [Key]
        public int TimeIntervalId { get; init; }
        [Required]
        public string Name { get; set; }

        // TODO: Check that this works with both FK on the TaskSchedule entity
        [InverseProperty("TriggerOffsetAmountTimeInterval")]
        public virtual ICollection<TaskScheduleEntity> TriggerOffsetTaskSchedules { get; set; }
        [InverseProperty("IntervalAmountTimeInterval")]
        public virtual ICollection<TaskScheduleEntity> IntervalTaskSchedules { get; set; }
    }
}
