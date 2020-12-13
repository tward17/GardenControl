using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ControlDeviceDto
    {
        public int ControlDeviceId { get; init; }
        public DeviceType DeviceTypeId { get; set; }
        public string DeviceType { get { return Enum.GetName(typeof(DeviceType), DeviceTypeId); } }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
        public string SerialNumber { get; set; }
        public int? DefaultState { get; set; }
    }
}
