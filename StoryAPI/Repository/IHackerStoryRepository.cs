namespace StoryAPI.Repository
{
    public interface IHackerStoryRepository
    {
        /// <summary>
        /// Method Declaration
        /// </summary>
        public  Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageSize);
    }
}
