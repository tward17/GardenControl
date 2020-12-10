using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class AppSetting
    {
        public int ConfigId { get; init; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
