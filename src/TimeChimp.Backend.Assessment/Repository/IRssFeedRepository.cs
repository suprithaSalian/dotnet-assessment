using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DataModel;

namespace TimeChimp.Backend.Assessment.Repository
{
    public interface IRssFeedRepository
    {
        Task UploadRssFeed(rssChannelItem[] listOfItems);

        Task<List<RssItem>> ReadTop5RssFeed();
        Task<List<RssItem>> ReadRssFeedOfLast(int days);
    }
}
