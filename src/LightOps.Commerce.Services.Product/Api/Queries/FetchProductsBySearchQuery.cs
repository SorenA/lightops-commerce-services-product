using LightOps.Commerce.Proto.Types;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsBySearchQuery : IQuery
    {
        /// <summary>
        /// The term to search for, if any
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Search only in localized strings with a specific language code, if any specified.
        /// ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK
        /// </summary>
        public string LanguageCode { get; set; }

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

        /// <summary>
        /// The currency code to use for sorting if sorting by currency
        /// ISO 4217 3-letter currency code
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}