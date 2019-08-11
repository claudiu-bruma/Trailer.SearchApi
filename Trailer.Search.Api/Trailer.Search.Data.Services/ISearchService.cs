using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trailer.Search.Data.Services.SearchResults;

namespace Trailer.Search.Data.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<TrailerSearchResult>> Search(string movieName);
    }
}
