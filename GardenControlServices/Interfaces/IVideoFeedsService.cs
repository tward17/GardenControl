using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface IVideoFeedsService
    {
        public Task<VideoFeed> InsertVideoFeedAsync(VideoFeed videoFeed);
        public Task<IEnumerable<VideoFeed>> GetAllVideoFeedsAsync();
        public Task<VideoFeed> GetVideoFeedByIdAsync(int id);
        public Task DeleteVideoFeedAsync(int id);
        public Task<VideoFeed> UpdateVideoFeedAsync(VideoFeed videoFeed);
    }
}
