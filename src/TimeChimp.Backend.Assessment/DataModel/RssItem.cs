using System;

namespace TimeChimp.Backend.Assessment.DataModel
{
    public class RssItem
    {
        public int RssId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Guid { get; set; }
        public string Enclosure { get; set; }
        public string Category { get; set; }
        public string Rights { get; set;}

    }
}
