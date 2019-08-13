using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trailer.Search.Data.Services;
using Trailer.Search.Data.Services.SearchResults;
using Trailer.Search.Infrastructure;

namespace Trailer.Search.Data.Services.Youtube
{
    public class YoutubeSearch : IVideoServiceSearch
    {
        private const string part = "snippet";
        private const string videoType = "videos";
        private const string trailerSelectorPrefix = "trailer";
        private const int defaultMaxNumberOfResults = 50;
        private AppSettings settings;
        public YouTubeService YouTubeService { get; set; }

        public YoutubeSearch(IOptions<AppSettings> settings , YouTubeService youTubeService)
        {
            this.settings = settings.Value;
            this.YouTubeService = youTubeService;
        }

        public async Task<IEnumerable<TrailerSearchResult>> Search(string query)
        { 
            var searchListRequest = YouTubeService.Search.List(part);
            searchListRequest.Q = $"{trailerSelectorPrefix} {query}";
            searchListRequest.Type = videoType;

            searchListRequest.MaxResults = defaultMaxNumberOfResults; 
            var searchListResponse = await searchListRequest.ExecuteAsync();


            return searchListResponse.Items.Select(x => new TrailerSearchResult() {
                ThumbnailUrl = x.Snippet.Thumbnails.Medium.Url,
                Title =  x.Snippet.Title,
                Url = $@"{settings.YoutubeUrlPrefix}{x.Id.VideoId}",
                Uid =  x.Id.VideoId
            }).ToList();
        }
    }
   
}
