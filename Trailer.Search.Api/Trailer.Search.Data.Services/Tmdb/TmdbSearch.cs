using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Trailer.Search.Data.Services;
using Trailer.Search.Data.Services.SearchResults;
using Trailer.Search.Infrastructure;

namespace Trailer.Search.Data.Services.Tmdb
{
    public class TmdbSearch : IMovieDatabaseSearch
    {
        public TMDbClient TmbdClient { get; set; }

        private const string trailerType = "Trailer";
        private AppSettings settings;
        public TmdbSearch(IOptions<AppSettings> settings, TMDbClient tmdbClient )
        {
            this.settings = settings.Value;
            this.TmbdClient = tmdbClient;
        }

        public async Task<IEnumerable<TrailerSearchResult>> Search(string movieName)
        {

            SearchContainer<SearchMovie> results = TmbdClient.SearchMovieAsync(movieName).Result;

            var trailers = new List<TrailerSearchResult>();
            foreach (var item in results.Results)
            {
                trailers.AddRange(await FillIntrailerUrl(item));
            }

            return trailers;

        }

        private async Task<IEnumerable<TrailerSearchResult>> FillIntrailerUrl(SearchMovie item)
        {
            var movieDetails = await TmbdClient.GetMovieAsync(item.Id, MovieMethods.Videos);
            if (movieDetails.Videos.Results.Any(x => x.Type == trailerType))
            {

                return movieDetails.Videos.Results.Select(x => new TrailerSearchResult()
                {
                    ThumbnailUrl = $"{settings.TmdbPosterPrefix}{item.PosterPath}",
                    Title = $"{item.Title} - {x.Name}",
                    Url = $"{settings.YoutubeUrlPrefix}{x.Key}",
                    Uid = x.Id

                });
            }
            return new List<TrailerSearchResult>();


        }
    }
}
