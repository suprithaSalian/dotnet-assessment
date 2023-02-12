using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeChimp.Backend.Assessment.Repository
{
    public interface INuApi
    {
        [Get("/rss")]
        Task <rss> GetRss();
    }
}
