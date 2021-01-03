using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class RelaySetStateDto
    {
        public int DeviceId { get; set; }
        public RelayState State { get; set; }
    }
}
