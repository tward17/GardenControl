using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Models
{
    public class AppSettingDto
    {
        [Required]
        public int AppSettingId { get; init; }

        [Required]
        [MaxLength(255)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public bool CanBeUpdated { get; init; }
    }
}
