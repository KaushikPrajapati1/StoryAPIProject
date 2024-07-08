using HackerStoryBusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerStoryBusinessLayer.Repository
{
    public interface IHackerStoryRepository
    {
        /// <summary>
        /// Method Declaration
        /// </summary>
        public Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageSize);
    }
}
