using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Enums;
using LightOps.Commerce.Services.Product.Api.Models;
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
        
        public Task<SearchResult<IProduct>> HandleAsync(FetchProductsBySearchQuery query)
        {

            var inMemoryQuery = _inMemoryProductProvider.Products
                .AsQueryable();

            // Sort underlying list
            switch (query.SortKey)
            {
                case ProductSortKey.Title:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.Title);
                    break;
                case ProductSortKey.CreatedAt:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.CreatedAt);
                    break;
                case ProductSortKey.UpdatedAt:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.UpdatedAt);
                    break;
                case ProductSortKey.UnitPrice:
                    inMemoryQuery = inMemoryQuery.OrderBy(x => x.Variants.Min(v => v.UnitPrice));
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
                var searchTerm = query.SearchTerm.ToLowerInvariant();
                inMemoryQuery = inMemoryQuery
                    .Where(x =>
                        (!string.IsNullOrWhiteSpace(x.Title) && x.Title.ToLowerInvariant().Contains(searchTerm))
                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLowerInvariant().Contains(searchTerm))
                        || x.Variants.Any(v =>
                            !string.IsNullOrWhiteSpace(v.Title) && v.Title.ToLowerInvariant().Contains(searchTerm)));
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
                .Select(x => new CursorNodeResult<IProduct>
                {
                    Cursor = EncodeCursor(x.Id),
                    Node = x,
                })
                .ToList();

            // Get cursors
            var startCursor = results.FirstOrDefault()?.Cursor;
            var endCursor = results.LastOrDefault()?.Cursor;

            var searchResult = new SearchResult<IProduct>
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