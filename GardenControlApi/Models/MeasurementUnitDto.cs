using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class MeasurementUnitDto
    {
        public int MeasurementUnitId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
