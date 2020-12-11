using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenControlRepositories.Entities
{
    [Table("ControlDevice")]
    public class ControlDeviceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ControlDeviceId { get; init; }
        
        [Required]
        public int DeviceTypeId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
    }
}
