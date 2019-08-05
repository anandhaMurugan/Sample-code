using System;
using System.Collections.Generic;

namespace Helseboka.Core.Common.Model
{
    public class PaginationResponse<T> where T : class, new()
    {
        public List<T> Content { get; set; }

        public PageableModel Pageable { get; set; }
        public bool Last { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool First { get; set; }
        public SortModel Sort { get; set; }
        public int NumberOfElements { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
    }

    public class SortModel
    {
        public bool Sorted { get; set; }
        public bool Unsorted { get; set; }
    }

    public class PageableModel
    {
        public SortModel Sort { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Offset { get; set; }
        public bool Paged { get; set; }
        public bool Unpaged { get; set; }
    }
}
