using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class DS18B20Dto
    {
        public DateTime MeasurementDateTime { get; set; }
        public decimal MeasurementValueCelcius { get; set; }
        public decimal MeasurementValueFahrenheit { get; set; }
    }
}
