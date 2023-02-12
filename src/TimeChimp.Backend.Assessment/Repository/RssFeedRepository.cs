using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DataModel;

namespace TimeChimp.Backend.Assessment.Repository
{
    public class RssFeedRepository : IRssFeedRepository
    {
        private readonly RssDbContext _rssDbContext;

        public RssFeedRepository(RssDbContext rssDbContext)
        {
            _rssDbContext = rssDbContext;
            
        }

        public async Task UploadRssFeed(rssChannelItem[] listOfItems)
        {
            if (listOfItems == null || !listOfItems.Any())
                return;
            foreach(var item in listOfItems)
            {
                var alreadyExists = _rssDbContext.Items.Any(x => x.Guid == item.guid.Value);
                if (!alreadyExists)
                {
                    var rssitem = new RssItem();
                    rssitem.Title = item.title;
                    rssitem.Description = item.description;
                    rssitem.PubDate = DateTime.Parse(item.pubDate);
                    rssitem.Category = item.category[0];
                    rssitem.Enclosure = item.enclosure.url;
                    rssitem.Link = item.link;
                    rssitem.Guid = item.guid.Value;
                    rssitem.Rights = item.rights;
                    await _rssDbContext.Items.AddAsync(rssitem);
                    
                }
            }
            await _rssDbContext.SaveChangesAsync();
        }

        public async Task<List<RssItem>> ReadRssFeedOfLast(int days)
        {
           return _rssDbContext.Items.OrderByDescending(a => a.PubDate).Take(days).ToList();
        }

        public async Task<List<RssItem>> ReadTop5RssFeed()
        {
            return _rssDbContext.Items.Take(5).ToList();
        }
    }
}
