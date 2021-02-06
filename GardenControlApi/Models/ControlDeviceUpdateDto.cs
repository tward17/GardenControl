using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ControlDeviceUpdateDto
    {
        [Required]
        [MaxLength(255)]
        public string Alias { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
        public string SerialNumber { get; set; }
        public DefaultState? DefaultState { get; set; }
    }
}
