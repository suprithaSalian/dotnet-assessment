using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DataModel;

namespace TimeChimp.Backend.Assessment.Services
{
    public interface IRssFeedService
    {
        Task<List<RssItem>> GetTop5RssFeed();
        Task<List<RssItem>> GetRssFeedOfLast(int days);

    }
}
