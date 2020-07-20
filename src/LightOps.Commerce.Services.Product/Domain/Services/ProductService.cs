using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
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

        public Task<IProduct> GetByIdAsync(string id)
        {
            return _queryDispatcher.DispatchAsync<FetchProductByIdQuery, IProduct>(new FetchProductByIdQuery
            {
                Id = id,
            });
        }

        public Task<IList<IProduct>> GetByIdAsync(IList<string> ids)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByIdsQuery, IList<IProduct>>(new FetchProductsByIdsQuery
            {
                Ids = ids,
            });
        }

        public Task<IProduct> GetByHandleAsync(string handle)
        {
            return _queryDispatcher.DispatchAsync<FetchProductByHandleQuery, IProduct>(new FetchProductByHandleQuery
            {
                Handle = handle,
            });
        }

        public Task<IList<IProduct>> GetByHandleAsync(IList<string> handles)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByHandlesQuery, IList<IProduct>>(new FetchProductsByHandlesQuery
            {
                Handles = handles,
            });
        }

        public Task<IList<IProduct>> GetByCategoryIdAsync(string categoryId)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByCategoryIdQuery, IList<IProduct>>(new FetchProductsByCategoryIdQuery
            {
                CategoryId = categoryId,
            });
        }

        public Task<IDictionary<string, IList<IProduct>>> GetByCategoryIdAsync(IList<string> categoryIds)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsByCategoryIdsQuery, IDictionary<string, IList<IProduct>>>(new FetchProductsByCategoryIdsQuery
            {
                CategoryIds = categoryIds,
            });
        }

        public Task<IList<IProduct>> GetBySearchAsync(string searchTerm)
        {
            return _queryDispatcher.DispatchAsync<FetchProductsBySearchQuery, IList<IProduct>>(new FetchProductsBySearchQuery
            {
                SearchTerm = searchTerm,
            });
        }
    }
}