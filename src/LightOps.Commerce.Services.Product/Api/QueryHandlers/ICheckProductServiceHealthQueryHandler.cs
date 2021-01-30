using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.CQRS.Api.Queries;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Api.QueryHandlers
{
    public interface ICheckProductServiceHealthQueryHandler : IQueryHandler<CheckProductServiceHealthQuery, HealthStatus>
    {

    }
}