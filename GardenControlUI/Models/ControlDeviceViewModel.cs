using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlUI.Models
{
    public class ControlDeviceViewModel
    {
        public int ControlDeviceId { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string DeviceType { get; set; }
        public int? GPIOPinNumber { get; set; }
        public bool IsActive { get; set; }
        public string SerialNumber { get; set; }
        public string DefaultState { get; set; }
    }
}
