using System;

namespace Trailer.Search.Infrastructure
{
    public class AppSettings
    {
        public string YoutubeApiKey { get; set; }

        public string TmdbApiKey { get; set; }
        public string TmdbPosterPrefix { get; set; }

        public string YoutubeUrlPrefix { get; set; }
    }
}
