using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class MeasurementDto
    {
        public long MeasurementId { get; init; }

        public int ControlDeviceId { get; init; }

        public double MeasurementValue { get; set; }

        public MeasurementUnit MeasurementUnit { get; set; }

        public DateTime MeasurementDateTime { get; set; }
    }
}
