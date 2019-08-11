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
    public class ValuesController : ControllerBase
    {
        public ISearchService SearchService { get; set; }
        public ValuesController(ISearchService searchService)
        {
            this.SearchService = searchService;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<TrailerSearchResult>> Get()
        {
       

           var trailerResults= await SearchService.Search("coco");

         
            return trailerResults;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
