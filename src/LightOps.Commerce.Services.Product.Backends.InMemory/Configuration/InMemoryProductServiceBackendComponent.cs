using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Domain.Providers;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Configuration
{
    public class InMemoryProductServiceBackendComponent : IDependencyInjectionComponent, IInMemoryProductServiceBackendComponent
    {
        public string Name => "lightops.commerce.services.product.backend.in-memory";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            // Populate in-memory providers
            _providers[Providers.InMemoryProductProvider].ImplementationInstance = new InMemoryProductProvider
            {
                Products = _products,
            };

            return new List<ServiceRegistration>()
                .Union(_providers.Values)
                .ToList();
        }

        #region Entities
        private readonly IList<IProduct> _products = new List<IProduct>();

        public IInMemoryProductServiceBackendComponent UseProducts(IList<IProduct> products)
        {
            foreach (var product in products)
            {
                _products.Add(product);
            }

            return this;
        }
        #endregion Entities

        #region Providers
        internal enum Providers
        {
            InMemoryProductProvider,
        }

        private readonly Dictionary<Providers, ServiceRegistration> _providers = new Dictionary<Providers, ServiceRegistration>
        {
            [Providers.InMemoryProductProvider] = ServiceRegistration.Singleton<IInMemoryProductProvider>(),
        };
        #endregion Providers
    }
}