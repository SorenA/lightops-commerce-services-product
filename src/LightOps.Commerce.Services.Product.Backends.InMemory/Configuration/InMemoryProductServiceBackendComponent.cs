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
            return new List<ServiceRegistration>()
                .Union(_providers.Values)
                .ToList();
        }

        #region Entities
        public IInMemoryProductServiceBackendComponent UseProducts(IList<IProduct> products)
        {
            // Populate in-memory providers
            _providers[Providers.InMemoryProductProvider].ImplementationType = null;
            _providers[Providers.InMemoryProductProvider].ImplementationInstance = new InMemoryProductProvider
            {
                Products = products,
            };

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
            [Providers.InMemoryProductProvider] = ServiceRegistration.Singleton<IInMemoryProductProvider, InMemoryProductProvider>(),
        };

        public IInMemoryProductServiceBackendComponent OverrideProductProvider<T>() where T : IInMemoryProductProvider
        {
            _providers[Providers.InMemoryProductProvider].ImplementationInstance = null;
            _providers[Providers.InMemoryProductProvider].ImplementationType = typeof(T);
            return this;
        }
        #endregion Providers
    }
}