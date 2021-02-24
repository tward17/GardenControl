using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("ScheduleTask")]
    public class ScheduleTaskEntity
    {
        [Key]
        public int ScheduleTaskId { get; init; }

        [Required]
        [ForeignKey("Schedule")]
        public int SheduleId { get; set; }
        public virtual ScheduleEntity Schedule { get; set; }

        [Required]
        public TaskActionId TaskActionId { get; set; }

        [Required]
        [ForeignKey("ControlDevice")]
        public int ControlDeviceId { get; set; }
        public virtual ControlDeviceEntity ControlDevice { get; set; }

        public bool IsActive { get; set; }
    }
}
