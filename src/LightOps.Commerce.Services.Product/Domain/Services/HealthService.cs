using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.CQRS.Api.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Domain.Services
{
    public class HealthService : IHealthService
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public HealthService(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public Task<HealthStatus> CheckProduct()
        {
            return _queryDispatcher.DispatchAsync<CheckProductHealthQuery, HealthStatus>(new CheckProductHealthQuery());
        }
    }
}