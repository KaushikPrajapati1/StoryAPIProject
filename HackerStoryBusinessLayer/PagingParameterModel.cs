using HackerStoryBusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerStoryBusinessLayer.Model
{
    public class PagingParameterModel
    {
        // const int maxPageSize = 300;

        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; }

        public int totalCount { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
        public List<HackerStory> hackerstory { get; set; }
    }
}