using AutoMapper;
using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class VideoFeedsService : IVideoFeedsService
    {
        private IVideoFeedsRepository _videoFeedRepository { get; init; }
        private ILogger<VideoFeedsService> _logger { get; init; }
        private IMapper _mapper { get; init; }


        public VideoFeedsService(IVideoFeedsRepository videoFeedRepository, ILogger<VideoFeedsService> logger, IMapper mapper)
        {
            _videoFeedRepository = videoFeedRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task DeleteVideoFeedAsync(int id)
        {
            await _videoFeedRepository.DeleteVideoFeedAsync(id);
        }

        public async Task<IEnumerable<VideoFeed>> GetAllVideoFeedsAsync()
        {
            return _mapper.Map<IEnumerable<VideoFeed>>(await _videoFeedRepository.GetAllVideoFeedsAsync());
        }

        public async Task<VideoFeed> GetVideoFeedByIdAsync(int id)
        {
            return _mapper.Map<VideoFeed>(await _videoFeedRepository.GetVideoFeedByIdAsync(id));
        }

        public async Task<VideoFeed> InsertVideoFeedAsync(VideoFeed videoFeed)
        {
            if (videoFeed == null)
                throw new ArgumentNullException(nameof(videoFeed));

            var videoFeedEntity = _mapper.Map<VideoFeedEntity>(videoFeed);

            try
            {
                await _videoFeedRepository.InsertVideoFeedAsync(videoFeedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting new Video Feed, {ex.Message}");
                throw;
            }

            return _mapper.Map<VideoFeed>(videoFeedEntity);
        }

        public async Task<VideoFeed> UpdateVideoFeedAsync(VideoFeed videoFeed)
        {
            if (videoFeed == null)
                throw new ArgumentNullException(nameof(videoFeed));

            var videoFeedEntity = await _videoFeedRepository.GetVideoFeedByIdAsync(videoFeed.VideoFeedId);

            //TODO: Better exception
            if (videoFeedEntity == null)
                throw new Exception();

            videoFeedEntity.Name = videoFeed.Name;
            videoFeedEntity.FeedUrl = videoFeed.FeedUrl;
            videoFeedEntity.IsActive = videoFeed.IsActive;
            videoFeedEntity.Description = videoFeed.Description;

            try
            {
                await _videoFeedRepository.UpdateVideoFeedAsync(videoFeedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Video Feed: {videoFeed.VideoFeedId}, {ex.Message}");
                throw;
            }

            return _mapper.Map<VideoFeed>(videoFeedEntity);
        }
    }
}
