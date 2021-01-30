using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class CheckProductServiceHealthQueryHandler : ICheckProductServiceHealthQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public CheckProductServiceHealthQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<HealthStatus> HandleAsync(CheckProductServiceHealthQuery query)
        {
            return _inMemoryProductProvider.Products != null
                ? Task.FromResult(HealthStatus.Healthy)
                : Task.FromResult(HealthStatus.Unhealthy);
        }
    }
}