using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductsBySearchQueryHandler : IFetchProductsBySearchQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductsBySearchQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }
        
        public Task<SearchResult<Proto.Types.Product>> HandleAsync(FetchProductsBySearchQuery query)
        {

            var inMemoryQuery = _inMemoryProductProvider
                .Products?
                .AsQueryable() ?? new EnumerableQuery<Proto.Types.Product>(new List<Proto.Types.Product>());

            // Match currency code if requested
            if (!string.IsNullOrEmpty(query.CurrencyCode))
            {
                // Limit to products with at least one variant with a matching unit price
                inMemoryQuery = inMemoryQuery
                    .Where(x => x.Variants
                        .Any(v => v.UnitPrices
                            .Any(p => p.CurrencyCode == query.CurrencyCode)));
            }

            // Use default sorting order
            inMemoryQuery = inMemoryQuery.OrderBy(x => x.SortOrder);

            // Sort underlying list if sorting key is specified
            switch (query.SortKey)
            {
                case ProductSortKey.Title:
                    // Only possible when using a language code
                    if (!string.IsNullOrEmpty(query.LanguageCode))
                    {
                        inMemoryQuery = inMemoryQuery.OrderBy(x => x.Titles
                            .Where(l => l.LanguageCode == query.LanguageCode) // Match language code
                            .Select(l => l.Value) // Convert to value
                            .FirstOrDefault());
                    }
                    break;
                case ProductSortKey.CreatedAt:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.CreatedAt.ToDateTime());
                    break;
                case ProductSortKey.UpdatedAt:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.UpdatedAt.ToDateTime());
                    break;
                case ProductSortKey.UnitPrice:
                    // Only possible when using a currency code
                    if (!string.IsNullOrEmpty(query.CurrencyCode))
                    {
                        inMemoryQuery = inMemoryQuery.OrderBy(x =>
                            x.Variants
                                .Min(v => v.UnitPrices
                                    .Where(p => p.CurrencyCode == query.CurrencyCode) // Match currency code
                                    .Select(p => p.Units + p.Nanos / 1_000_000_000) // Convert to long
                                    .FirstOrDefault()));
                    }
                    break;
            }

            // Reverse underlying list if requested
            if (query.Reverse)
            {
                inMemoryQuery = inMemoryQuery.Reverse();
            }

            // Match category id if requested
            if (!string.IsNullOrEmpty(query.CategoryId))
            {
                inMemoryQuery = inMemoryQuery.Where(x => x.CategoryIds.Contains(query.CategoryId));
            }

            // Search in list
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                // Match language if requested
                if (!string.IsNullOrEmpty(query.LanguageCode))
                {
                    inMemoryQuery = inMemoryQuery
                        .Where(x =>
                            x.Titles.Any(l =>
                                l.LanguageCode == query.LanguageCode
                                && l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                    StringComparison.InvariantCultureIgnoreCase))
                            || x.Descriptions.Any(l =>
                                l.LanguageCode == query.LanguageCode
                                && l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                    StringComparison.InvariantCultureIgnoreCase))
                            || x.Variants.Any(
                                v => v.Titles.Any(l =>
                                    l.LanguageCode == query.LanguageCode
                                    && l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                        StringComparison.InvariantCultureIgnoreCase))));
                }
                else
                {
                    // No language code, match all
                    inMemoryQuery = inMemoryQuery
                        .Where(x =>
                            x.Titles.Any(l =>
                                l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                    StringComparison.InvariantCultureIgnoreCase))
                            || x.Descriptions.Any(l =>
                                l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                    StringComparison.InvariantCultureIgnoreCase))
                            || x.Variants.Any(
                                v => v.Titles.Any(l =>
                                    l.Value.ToLowerInvariant().Contains(query.SearchTerm,
                                        StringComparison.InvariantCultureIgnoreCase))));
                }
            }

            // Get total results
            var totalResults = inMemoryQuery.Count();

            // Paginate - Skip
            var nodeId = DecodeCursor(query.PageCursor);
            var remainingResultsPrePagination = inMemoryQuery.Count();
            if (!string.IsNullOrEmpty(nodeId))
            {
                // Skip until we reach cursor, then one more for next page
                inMemoryQuery = inMemoryQuery
                    .SkipWhile(x => x.Id != nodeId)
                    .Skip(1);
            }

            // Get remaining results to know if next page is available
            var remainingResults = inMemoryQuery.Count();

            // Paginate - Take
            var results = inMemoryQuery
                .Take(query.PageSize)
                .Select(x => new CursorNodeResult<Proto.Types.Product>
                {
                    Cursor = EncodeCursor(x.Id),
                    Node = x,
                })
                .ToList();

            // Get cursors
            var startCursor = results.FirstOrDefault()?.Cursor;
            var endCursor = results.LastOrDefault()?.Cursor;

            var searchResult = new SearchResult<Proto.Types.Product>
            {
                Results = results,
                StartCursor = startCursor,
                EndCursor = endCursor,
                HasNextPage = remainingResults > query.PageSize,
                HasPreviousPage = remainingResultsPrePagination != remainingResults,
                TotalResults = totalResults,
            };

            return Task.FromResult(searchResult);
        }

        private string EncodeCursor(string rawCursor)
        {
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(rawCursor);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                // Invalid cursor
                return string.Empty;
            }
        }

        private string DecodeCursor(string encodedCursor)
        {
            try
            {
                var bytes = Convert.FromBase64String(encodedCursor);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // Invalid cursor
                return string.Empty;
            }
        }
    }
}