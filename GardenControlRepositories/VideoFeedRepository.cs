using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories
{
    public class VideoFeedRepository : IVideoFeedsRepository
    {
        private GardenControlContext _context { get; init; }
        private ILogger<VideoFeedRepository> _logger { get; init; }

        public VideoFeedRepository(GardenControlContext context, ILogger<VideoFeedRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteVideoFeedAsync(int id)
        {
            var videoFeedEntity = await _context.VideoFeedEntities
                .Where(x => x.VideoFeedId == id).FirstOrDefaultAsync();

            if (videoFeedEntity == null)
            {
                _logger.LogWarning($"Tried deleting Video Feed that does not exist, VideoFeedId: {id}");
                return;
            }

            try
            {
                _context.VideoFeedEntities.Remove(videoFeedEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Video Feed ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<VideoFeedEntity>> GetAllVideoFeedsAsync()
        {
            return await _context.VideoFeedEntities.ToListAsync();
        }

        public async Task<VideoFeedEntity> GetVideoFeedByIdAsync(int id)
        {
            return await _context.VideoFeedEntities
                .Where(x => x.VideoFeedId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<VideoFeedEntity> InsertVideoFeedAsync(VideoFeedEntity videoFeed)
        {
            if (videoFeed == null)
                throw new NullReferenceException(nameof(videoFeed));

            try
            {
                _context.VideoFeedEntities.Add(videoFeed);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting Video Feed: {ex.Message}");
                throw;
            }

            return videoFeed;
        }

        public async Task<VideoFeedEntity> UpdateVideoFeedAsync(VideoFeedEntity videoFeed)
        {
            if (videoFeed == null)
                throw new NullReferenceException(nameof(videoFeed));

            try
            {
                _context.Entry(videoFeed).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Video Feed: {videoFeed.VideoFeedId}, {ex.Message}");
                throw;
            }

            return videoFeed;
        }
    }
}
