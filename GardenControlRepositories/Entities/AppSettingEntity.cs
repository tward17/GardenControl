using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("AppSetting")]
    public class AppSettingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfigId { get; init; }

        [Required]
        [MaxLength(255)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
