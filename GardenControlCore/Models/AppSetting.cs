﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class AppSetting
    {
        public int AppSettingId { get; set; }
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
        public bool CanBeUpdated { get; set; }
    }
}
