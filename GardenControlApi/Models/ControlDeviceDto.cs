using GardenControlCore.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class ControlDeviceDto
    {
        public int ControlDeviceId { get; init; }
        public DeviceType DeviceTypeId { get; set; }
        public string DeviceType { get { return Enum.GetName(typeof(DeviceType), DeviceTypeId); } }
        [Required]
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? GPIOPinNumber { get; set; }
        public string SerialNumber { get; set; }
        public DefaultState? DefaultState { get; set; }
    }
}
