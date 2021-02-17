﻿using GardenControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class Measurement
    {
        public long MeasurementId { get; init; }

        public int ControlDeviceId { get; init; }

        public virtual ControlDevice ControlDevice { get; set; }

        public double MeasurementValue { get; set; }

        public MeasurementUnit MeasurementUnit { get; set; }

        public DateTime MeasurementDateTime { get; set; }
    }
}
