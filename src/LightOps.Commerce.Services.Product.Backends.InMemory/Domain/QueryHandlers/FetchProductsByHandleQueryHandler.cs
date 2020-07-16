using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductsByHandleQueryHandler : IFetchProductsByHandleQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductsByHandleQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<IList<IProduct>> HandleAsync(FetchProductsByHandleQuery query)
        {
            var products = _inMemoryProductProvider
                .Products
                .Where(c => query.Handles.Contains(c.Handle))
                .ToList();

            return Task.FromResult<IList<IProduct>>(products);
        }
    }
}