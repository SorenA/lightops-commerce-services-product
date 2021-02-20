using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductsByHandlesQueryHandler : IFetchProductsByHandlesQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductsByHandlesQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<IList<Proto.Types.Product>> HandleAsync(FetchProductsByHandlesQuery query)
        {
            // Match any localized handle
            var products = _inMemoryProductProvider
                .Products?
                .Where(p => p.Handles
                    .Select(ls => ls.Value)
                    .Intersect(query.Handles)
                    .Any())
                .ToList();

            return Task.FromResult<IList<Proto.Types.Product>>(products ?? new List<Proto.Types.Product>());
        }
    }
}