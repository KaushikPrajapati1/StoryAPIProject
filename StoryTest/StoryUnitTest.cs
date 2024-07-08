using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using HackerStoryBusinessLayer;
using HackerStoryBusinessLayer.Repository;
using System.Security.Cryptography;

namespace StoryTest
{
    public class StoryUnitTest
    {


        private IHackerStoryRepository hackerStoryRepository { get; set; }
        public StoryUnitTest()
        {


        }
        [Fact]
        public void Test()
        {

            [Fact]
            async Task Task_GetHackerStories_Return_OkResult()
            {
                // arrange
                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository, cache);

                // act
                var result = await controller.GetHackerStories(5);
                var okResult = result as OkObjectResult;

                // assert
                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);
            }
            [Fact]
            async Task Task_GetHackerStories_Return_BadResult()
            {
                // arrange
                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository, cache);

                // act
                var result = await controller.GetHackerStories(0);
                var okResult = result as NotFoundResult;

                // assert
                Assert.NotNull(okResult);
                Assert.Equal(404, okResult.StatusCode);
            }
         

            [Fact]
            void Task_GetHackerStories_Return_BadRequestResult1()
            {
                //Arrange  
                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository, cache);

                //Act  
                var data = controller.GetHackerStories(1);
                data = null;

                if (data != null)
                    //Assert  
                    Assert.IsType<BadRequestResult>(data);
            }
        }
    }
}