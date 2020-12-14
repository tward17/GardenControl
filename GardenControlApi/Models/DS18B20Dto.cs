using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class DS18B20Dto
    {
        public DateTime ReadingDateTime { get; set; }
        public decimal ReadingValueCelcius { get; set; }
        public decimal ReadingValueFahrenheit { get; set; }
    }
}
