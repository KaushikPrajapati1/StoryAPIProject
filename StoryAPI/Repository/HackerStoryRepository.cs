
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace StoryAPI.Repository
{
    public class HackerStoryRepository : IHackerStoryRepository
    {
        private readonly IMemoryCache _memoryCache;
        public HackerStoryRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public List<T> Deserialize<T>(string SerializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);
            return stuff;
        }

        /// <summary>
        ///     GetHackerStoriesByMemoryCache is a method to get Hacker Story List using Memory Cache if 
        ///     data on cahce is available otherwise will get data from data source
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagingParameterModel1> GetHackerStoriesByMemoryCache(int pageSize)
        {
            // parameter declaration
            var cacheKey = "storyist";
            var pageNumber = 1;
            PagingParameterModel1 paginationMetadata;
            // if cache available
            if (_memoryCache.TryGetValue(cacheKey, out paginationMetadata))
            {
            // if cache availabe but pagesize change by user action
                if (pageSize != paginationMetadata.pageSize)
                {
                    _memoryCache.Remove(cacheKey);
                }
            }
            //if cahce expired or not available then set new data into memory cache of PagingParameterModel(paginationMetadata) model
            if (!_memoryCache.TryGetValue(cacheKey, out paginationMetadata))
            {
                // get data into PagingParameterModel (paginationMetadata)
                paginationMetadata =
            await GetHackerStories(pageNumber, pageSize);
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                   AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(180),
                };
                _memoryCache.Set(cacheKey, paginationMetadata, cacheExpiryOptions);
            }
            return (paginationMetadata);
        }

        /// <summary>
        /// GetHackerStories Method is to get Fresh Story Lists (PagingParameterModel) from Data Source
        /// </summary>
        public async Task<PagingParameterModel1> GetHackerStories(int pageNumber, int pageSize)
        {

            List<int> reservationList = new List<int>();
            List<HackerStory> storyList = new List<HackerStory>();
            PagingParameterModel paginationMetadata = null;

            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(20) })
            {
                //   using (var response = await httpClient.GetAsync(" https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"))
                using (var response = await httpClient.GetAsync(" https://hacker-news.firebaseio.com/v0/newstories.json"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //dynamic dObject = JObject.Parse(apiResponse);
                    reservationList = Deserialize<int>(apiResponse);

                    int count = reservationList.Count();

                    // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                    int CurrentPage = pageNumber;

                    // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                    int PageSize = pageSize;

                    // Display TotalCount to Records to User  
                    int TotalCount = count;

                    // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                    // Returns List of Customer after applying Paging   
                    var items = reservationList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                    // if CurrentPage is greater than 1 means it has previousPage  
                    var previousPage = CurrentPage > 1 ? "Yes" : "No";

                    // if TotalPages is greater than CurrentPage means it has nextPage  
                    var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                    var tasks = new List<Task<HackerStory>>();

                    foreach (var item in items)
                    {
                        tasks.Add(GetHackerStoryId(item.ToString()));
                    }

                    foreach(var tsk in await Task.WhenAll(tasks))
                    {
                        storyList.Add(tsk);
                    }
                   // set values in Moodel
                    paginationMetadata = new PagingParameterModel1()
                    {
                        totalCount = TotalCount,
                        pageSize = PageSize,
                        currentPage = CurrentPage,
                        totalPages = TotalPages,
                        previousPage = previousPage,
                        nextPage = nextPage,
                        hackerstory = storyList
                    };
                }
            }
            return (paginationMetadata);
        }

        /// <summary>
        /// GetHackerStoryId Method is to get Fresh Story from Data Source by Id 
        /// </summary>
        public async Task<HackerStory> GetHackerStoryId(string Id)
        {
            HackerStory hackerStory = null;
            using (var httpClient = new HttpClient())
            {
                var strUrl = "https://hacker-news.firebaseio.com/v0/item/" + Id.ToString() + ".json?print=pretty";
                using (var response1 = await httpClient.GetAsync(strUrl))
                {
                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                    hackerStory = JsonConvert.DeserializeObject<HackerStory>(apiResponse1);
                }   
            }
            return (hackerStory);
        }
        
    }
}
