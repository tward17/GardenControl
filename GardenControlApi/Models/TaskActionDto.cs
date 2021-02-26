using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class TaskActionDto
    {
        public TaskActionId TaskActionId { get; set; }
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
