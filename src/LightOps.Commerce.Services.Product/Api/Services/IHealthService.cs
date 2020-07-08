using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Api.Services
{
    public interface IHealthService
    {
        Task<HealthStatus> CheckProduct();
    }
}