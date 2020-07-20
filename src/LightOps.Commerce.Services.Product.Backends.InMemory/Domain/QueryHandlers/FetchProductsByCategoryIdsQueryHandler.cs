using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductsByCategoryIdsQueryHandler : IFetchProductsByCategoryIdsQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductsByCategoryIdsQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<IDictionary<string, IList<IProduct>>> HandleAsync(FetchProductsByCategoryIdsQuery query)
        {
            var products = _inMemoryProductProvider
                .Products
                .Where(p => p.CategoryIds.Intersect(query.CategoryIds).Any())
                .ToList();

            var dictionary = new Dictionary<string, IList<IProduct>>();
            foreach (var categoryId in query.CategoryIds.Distinct())
            {
                var categoryProducts = products
                    .Where(p => p.CategoryIds.Contains(categoryId))
                    .ToList();

                dictionary.Add(categoryId, categoryProducts);
            }

            return Task.FromResult<IDictionary<string, IList<IProduct>>>(dictionary);
        }
    }
}