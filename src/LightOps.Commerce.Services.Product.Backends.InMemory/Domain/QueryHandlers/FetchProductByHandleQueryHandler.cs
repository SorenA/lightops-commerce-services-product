using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductByHandleQueryHandler : IFetchProductByHandleQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductByHandleQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<IProduct> HandleAsync(FetchProductByHandleQuery query)
        {
            var product = _inMemoryProductProvider
                .Products
                .FirstOrDefault(c => c.Handle == query.Handle);

            return Task.FromResult(product);
        }
    }
}