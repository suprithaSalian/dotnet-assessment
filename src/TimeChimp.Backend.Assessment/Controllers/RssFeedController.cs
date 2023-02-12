using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DataModel;
using TimeChimp.Backend.Assessment.Services;

namespace TimeChimp.Backend.Assessment.Controllers
{
    [Route("Api")]
    [ApiController]
    [AllowAnonymous]
    public class RssFeedController : ControllerBase
    {
        private readonly IRssFeedService _rssFeedService;

        public RssFeedController(IRssFeedService rssFeedService)
        {
            _rssFeedService = rssFeedService;
        }

        [HttpGet("Last/{days}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RssItem>>> RssFeedOfLast(int days)
        {
            if (days <= 0)
            {
                return BadRequest("Entered number of days is invalid.");
            }
            try
            {

                return await _rssFeedService.GetRssFeedOfLast(days); 
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("Top5RssFeed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RssItem>>> Top5RssFeed()
        {
            try
            {
                return await _rssFeedService.GetTop5RssFeed();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}