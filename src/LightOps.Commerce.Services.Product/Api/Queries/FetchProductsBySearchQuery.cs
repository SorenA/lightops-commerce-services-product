using LightOps.Commerce.Services.Product.Api.Enums;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsBySearchQuery : IQuery
    {
        /// <summary>
        /// The term to search for
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Search only in children with a specific category id, if any specified
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// The page cursor to use
        /// </summary>
        public string PageCursor { get; set; }

        /// <summary>
        /// The page size to use
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Sort the underlying list by the given key
        /// </summary>
        public ProductSortKey SortKey { get; set; }

        /// <summary>
        /// Whether to reverse the order of the underlying list
        /// </summary>
        public bool Reverse { get; set; }
    }
}