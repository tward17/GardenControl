using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlCore.Models
{
    public class VideoFeed
    {
        public int VideoFeedId { get; set; }
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
