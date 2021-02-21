using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenControlCore.Enums;

namespace GardenControlRepositories.Entities
{
    [Table("Measurement")]
    public class MeasurementEntity
    {
        [Key]
        public long MeasurementId { get; init; }

        [Required]
        [ForeignKey("ControlDeviceEntity")]
        public int ControlDeviceId { get; init; }
        
        public virtual ControlDeviceEntity ControlDeviceEntity { get; set; }
        
        [Required]
        public double MeasurementValue { get; set; }
        
        [Required]
        public MeasurementUnit MeasurementUnit { get; set; }
        
        [Required]
        public DateTime MeasurementDateTime { get; set; }
    }
}
