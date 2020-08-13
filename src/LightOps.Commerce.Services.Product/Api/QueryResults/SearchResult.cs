using System.Collections.Generic;

namespace LightOps.Commerce.Services.Product.Api.QueryResults
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Results = new List<CursorNodeResult<T>>();
        }

        /// <summary>
        /// The results found, if any
        /// </summary>
        public IList<CursorNodeResult<T>> Results { get; set; }

        /// <summary>
        /// The cursor of the first result
        /// </summary>
        public string StartCursor { get; set; }

        /// <summary>
        /// The cursor of the last result
        /// </summary>
        public string EndCursor { get; set; }

        /// <summary>
        /// Whether another page can be fetched
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Whether another page can be fetched
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// The total amount of results available
        /// </summary>
        public int TotalResults { get; set; }
    }
}