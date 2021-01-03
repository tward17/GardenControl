using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ControlDeviceInsertDto
    {
        [Required]
        public int DeviceTypeId { get; set; }
        [Required]
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
        public string SerialNumber { get; set; }
        public DefaultState? DefaultState { get; set; }
    }
}
