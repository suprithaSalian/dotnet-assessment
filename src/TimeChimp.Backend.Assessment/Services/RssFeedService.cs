using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DataModel;
using TimeChimp.Backend.Assessment.Repository;

namespace TimeChimp.Backend.Assessment.Services
{
    public class RssFeedService : IRssFeedService
    {
        private readonly IRssFeedRepository _rssFeedRepository;
        private readonly ILogger<RssFeedService> _logger;

        public RssFeedService(IRssFeedRepository rssFeedRepository, ILogger<RssFeedService> logger)
        {
            _rssFeedRepository = rssFeedRepository;
            _logger = logger;
        }
        public async Task<List<RssItem>> GetRssFeedOfLast(int days)
        {
            var rssfeed = await _rssFeedRepository.ReadRssFeedOfLast(days);
            if(rssfeed== null)
            {
                _logger.LogInformation("No Rss feeb found in the database");
            }
            return rssfeed;
        }

        public async Task<List<RssItem>> GetTop5RssFeed()
        {
            var rssfeed = await _rssFeedRepository.ReadTop5RssFeed();
            if(rssfeed == null)
            {
                _logger.LogError("No Rss feed in the database.");
            }
            return rssfeed;
        }
    }
}
