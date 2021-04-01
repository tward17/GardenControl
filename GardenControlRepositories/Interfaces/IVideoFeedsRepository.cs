using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    /// <summary>
    /// Performs CRUD operations for video feeds
    /// </summary>
    public interface IVideoFeedsRepository
    {
        public Task<VideoFeedEntity> InsertVideoFeedAsync(VideoFeedEntity videoFeed);
        public Task<IEnumerable<VideoFeedEntity>> GetAllVideoFeedsAsync();
        public Task<VideoFeedEntity> GetVideoFeedByIdAsync(int id);
        public Task DeleteVideoFeedAsync(int id);
        public Task<VideoFeedEntity> UpdateVideoFeedAsync(VideoFeedEntity videoFeed);
    }
}
