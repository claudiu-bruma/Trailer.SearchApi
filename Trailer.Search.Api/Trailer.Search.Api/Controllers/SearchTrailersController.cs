using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trailer.Search.Data.Services;
using Trailer.Search.Data.Services.SearchResults;
using Trailer.Search.Data.Services.Tmdb;
using Trailer.Search.Data.Services.Youtube;

namespace Trailer.Search.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public ISearchService SearchService { get; set; }
        public SearchController(ISearchService searchService)
        {
            this.SearchService = searchService;
        }
        // GET api/values
        [ResponseCache]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrailerSearchResult>>> Get(string query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return new BadRequestResult();
            }

            var trailerResults = await SearchService.Search(query);

            if (!trailerResults.Any())
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(trailerResults);
        }


    }
}
