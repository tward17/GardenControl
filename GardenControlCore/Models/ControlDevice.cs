using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class ControlDevice
    {
        public int ControlDeviceId { get; init; }
        public DeviceType DeviceTypeId { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
        public string SerialNumber { get; set; }
        public int? DefaultState { get; set; }
    }
}
