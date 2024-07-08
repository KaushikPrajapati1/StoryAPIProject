using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using StoryAPI.Repository;
using System.Collections;
using System.Text;
using System.Text.Json.Nodes;
using System.Web.Http;
using HackerStoryBusinessLayer.Repository;
using HackerStoryBusinessLayer.Model;
using HackerStoryBusinessLayer.Entity;
using IHackerStoryRepository = HackerStoryBusinessLayer.Repository.IHackerStoryRepository;

namespace StoryAPI.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    public class HackerStoryAPIController : ControllerBase
    {
        private IHackerStoryRepository hackerStoryRepository { get; set; }

        private readonly IMemoryCache _memoryCache;
        public class StoryData { public int storynumber; }



        public HackerStoryAPIController(IHackerStoryRepository hackerStoryRepository, IMemoryCache memoryCache)
        {
            this.hackerStoryRepository = hackerStoryRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// This method performs to Get Hacker Story List operation.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetHackerStories(int pageSize)
        {
            try
            {
                PagingParameterModel paginationMetadata;
                paginationMetadata =
            await hackerStoryRepository.GetHackerStoriesByMemoryCache(pageSize);

                if (paginationMetadata.pageSize == 0)
                {
                    return NotFound();
                }
                else
                    return Ok(paginationMetadata);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}