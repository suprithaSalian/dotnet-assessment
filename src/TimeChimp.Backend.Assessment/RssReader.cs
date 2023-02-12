using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Threading;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.Repository;

namespace TimeChimp.Backend.Assessment
{
    public class RssReader : BackgroundService
    {
        private readonly INuApi _nuApi;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IRssFeedRepository _rssFeedRepository;
        private readonly ILogger<RssReader> _logger;
        private readonly RssDbContext _rssContext;
        public RssReader(ILogger<RssReader> logger, INuApi nuApi, IServiceScopeFactory serviceScopeFactory)//, RssDbContext rssDbContext)
        {
            _logger = logger;
            _nuApi = nuApi;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using AsyncServiceScope asyncScope = _serviceScopeFactory.CreateAsyncScope();
            _rssFeedRepository = asyncScope.ServiceProvider.GetRequiredService<IRssFeedRepository>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var rss = await GetRssFeed();
                await UploadRssFeedToLocalDb(rss);
                _logger.LogInformation("Background service reading rss at: {time}", DateTimeOffset.Now);
                await Task.Delay(5*60*1000, stoppingToken);
            }
        }
        
        public async Task<rss> GetRssFeed()
        {
            try
            {
                var rss = await _nuApi.GetRss();
                return rss;
            }
            catch(ApiException ex)
            {
                _logger.LogError(ex, "Nu.nl returns unsucessful response ={statusCode} with messad {message}", ex.StatusCode, ex.Message);
                 throw;
            }
        }

        public async Task UploadRssFeedToLocalDb(rss rssFeed)
        {
            if (rssFeed == null || rssFeed.channel == null)
            {
                _logger.LogInformation("No Rss feed to read");
                return;
            }    
            await _rssFeedRepository.UploadRssFeed(rssFeed.channel.item);            
        }

      
    }
}