using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Entities
{
    [Table("VideoFeed")]
    public class VideoFeedEntity
    {
        [Key]
        public int VideoFeedId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FeedUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
