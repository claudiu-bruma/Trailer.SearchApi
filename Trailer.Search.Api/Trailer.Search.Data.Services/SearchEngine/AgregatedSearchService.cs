using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trailer.Search.Data.Services.SearchResults;
using Trailer.Search.Data.Services.Tmdb;
using Trailer.Search.Data.Services.Youtube;

namespace Trailer.Search.Data.Services.SearchEngine
{
    public class AgregatedSearchService : ISearchService
    {
        public IMovieDatabaseSearch MovieDatabaseSearch { get; set; }
        public IVideoServiceSearch VideoServiceSearch { get; set; }
        public AgregatedSearchService(IMovieDatabaseSearch movieDatabaseSearch , IVideoServiceSearch videoServiceSearch)
        {
            this.MovieDatabaseSearch =movieDatabaseSearch;
            this.VideoServiceSearch = videoServiceSearch;

        }
        public async Task<IEnumerable<TrailerSearchResult>> Search(string movieName)
        {
            var trailerList = new List<TrailerSearchResult>();
            trailerList.AddRange(await MovieDatabaseSearch.Search(movieName));
            trailerList.AddRange(await VideoServiceSearch.Search(movieName));

           return trailerList;
        }
    }
}
