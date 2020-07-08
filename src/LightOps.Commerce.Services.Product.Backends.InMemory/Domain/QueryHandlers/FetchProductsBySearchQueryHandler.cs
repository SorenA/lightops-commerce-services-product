using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
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
        
        public Task<IList<IProduct>> HandleAsync(FetchProductsBySearchQuery query)
        {
            var searchTerm = query.SearchTerm.ToLowerInvariant();

            var productQuery = _inMemoryProductProvider.Products
                .AsQueryable();

            if (query.SearchTerm != "*")
            {
                productQuery = productQuery
                    .Where(p =>
                        (string.IsNullOrWhiteSpace(p.Id) || p.Id.ToLowerInvariant().Contains(searchTerm))
                        || (string.IsNullOrWhiteSpace(p.Title) || p.Title.ToLowerInvariant().Contains(searchTerm))
                        || (string.IsNullOrWhiteSpace(p.Description) || p.Description.ToLowerInvariant().Contains(searchTerm))
                        || p.Variants.Any(pv =>
                            string.IsNullOrWhiteSpace(pv.Title) || pv.Title.ToLowerInvariant().Contains(searchTerm)));
            }

            return Task.FromResult<IList<IProduct>>(productQuery.ToList());
        }
    }
}