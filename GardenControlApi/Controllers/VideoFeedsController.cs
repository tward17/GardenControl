using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using GardenControlServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoFeedsController : Controller
    {
        private IVideoFeedsService _videoFeedsService { get; init; }
        private IMapper _mapper { get; init; }

        public VideoFeedsController(IMapper mapper, IVideoFeedsService videoFeedsService)
        {
            _mapper = mapper;
            _videoFeedsService = videoFeedsService;
        }

        /// <summary>
        /// Returns all Video Feeds
        /// </summary>
        /// <returns>Collection of Video Feeds</returns>
        /// <response code="200">Returns all Video Feeds</response>
        [HttpGet(Name = "VideoFeedsGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<VideoFeed>))]
        public async Task<IEnumerable<VideoFeed>> Get()
        {
            return await _videoFeedsService.GetAllVideoFeedsAsync();
        }

        /// <summary>
        /// Returns specified Video Feed
        /// </summary>
        /// <returns>Returns single Video Feed</returns>
        /// <response code="200">Returns specified Video Feed</response>
        /// <response code="404">Could not find Video Feed from id</response>
        [HttpGet("{id}", Name = "VideoFeedGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VideoFeed))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VideoFeed>> Get([FromRoute] int id)
        {
            if (!(await VideoFeedExists(id)))
                return NotFound();

            return await _videoFeedsService.GetVideoFeedByIdAsync(id);
        }

        /// <summary>
        /// Creates a Video Feed
        /// </summary>
        /// <returns>Returns created Video Feed</returns>
        /// <response code="201">Video Feed created successfully</response>
        [HttpPost(Name = "VideoFeedInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<VideoFeed>> Insert([FromBody] VideoFeed videoFeed)
        {
            var newVideoFeed = await _videoFeedsService.InsertVideoFeedAsync(videoFeed);
            return CreatedAtAction(nameof(Get), new { id = newVideoFeed.VideoFeedId }, newVideoFeed);
        }

        /// <summary>
        /// Updates specified Video Feed
        /// </summary>
        /// <returns>Updates specified Video Feed</returns>
        /// <response code="204">Video Feed updated successfully</response>
        /// <response code="400">Video Feed Id in url and object do not match</response>
        /// <response code="404">Could not find Video Feed from id</response>
        [HttpPut("{id}", Name = "VideoFeedUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] VideoFeed videoFeed)
        {
            if (!(await VideoFeedExists(id)))
                return NotFound();

            if (id != videoFeed.VideoFeedId)
                return BadRequest();

            await _videoFeedsService.UpdateVideoFeedAsync(videoFeed);

            return NoContent();
        }

        /// <summary>
        /// Deletes specified Video Feed
        /// </summary>
        /// <returns>Deletes single Video Feed</returns>
        /// <response code="204">Video Feed successfully deleted</response>
        /// <response code="404">Could not find Video Feed from id</response>
        [HttpDelete("{id}", Name = "VideoFeedDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!(await VideoFeedExists(id)))
                return NotFound();

            await _videoFeedsService.DeleteVideoFeedAsync(id);

            return NoContent();
        }

        private async Task<bool> VideoFeedExists(int id)
        {
            if (await _videoFeedsService.GetVideoFeedByIdAsync(id) != null)
                return true;

            return false;
        }
    }
}
