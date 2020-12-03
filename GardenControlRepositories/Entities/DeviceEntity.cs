using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenControlRepositories.Entities
{
    [Table("Device")]
    public class DeviceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; init; }
        
        [Required]
        public int DeviceTypeId { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int GPIOPinNumber { get; set; }
    }
}
