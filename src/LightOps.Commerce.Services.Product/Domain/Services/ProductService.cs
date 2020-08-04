using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Enums;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.CQRS.Api.Services;

namespace LightOps.Commerce.Services.Product.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ProductService(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public Task<IList<IProduct>> GetByHandleAsync(IList<string> handles)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByHandlesQuery, IList<IProduct>>(new FetchProductsByHandlesQuery
            {
                Handles = handles,
            });
        }

        public Task<IList<IProduct>> GetByIdAsync(IList<string> ids)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByIdsQuery, IList<IProduct>>(new FetchProductsByIdsQuery
            {
                Ids = ids,
            });
        }

        public Task<IList<IProduct>> GetBySearchAsync(string searchTerm)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsBySearchQuery, IList<IProduct>>(new FetchProductsBySearchQuery
            {
                SearchTerm = searchTerm,
            });
        }

        public Task<SearchResult<IProduct>> GetBySearchAsync(string searchTerm,
                                                             string categoryId,
                                                             string pageCursor,
                                                             int pageSize,
                                                             ProductSortKey sortKey,
                                                             bool reverse)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsBySearchQuery, SearchResult<IProduct>>(
                new FetchProductsBySearchQuery
                {
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = sortKey,
                    Reverse = reverse,
                });
        }
    }
}