using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("TaskAction")]
    public class TaskActionEntity
    {
        [Key]
        public int TaskActionId { get; init; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<TaskScheduleEntity> TaskSchedules { get; set; }
    }
}
