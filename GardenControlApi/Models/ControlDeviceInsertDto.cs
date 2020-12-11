using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ControlDeviceInsertDto
    {
        public int DeviceTypeId { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
    }
}
