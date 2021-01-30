using System;
using LightOps.Commerce.Services.Product.Backends.InMemory.Domain.CommandHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers;
using LightOps.Commerce.Services.Product.Configuration;
using LightOps.DependencyInjection.Configuration;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Configuration
{
    public static class ProductServiceComponentExtensions
    {
        public static IProductServiceComponent UseInMemoryBackend(
            this IProductServiceComponent serviceComponent,
            IDependencyInjectionRootComponent rootComponent,
            Action<IInMemoryProductServiceBackendComponent> componentConfig = null)
        {
            var component = new InMemoryProductServiceBackendComponent();

            // Invoke config delegate
            componentConfig?.Invoke(component);

            // Attach to root component
            rootComponent.AttachComponent(component);

            // Override command handlers
            serviceComponent
                .OverridePersistProductCommandHandler<PersistProductCommandHandler>()
                .OverrideDeleteProductCommandHandler<DeleteProductCommandHandler>();

            // Override query handlers
            serviceComponent
                .OverrideCheckProductServiceHealthQueryHandler<CheckProductServiceHealthQueryHandler>()
                .OverrideFetchProductsByHandlesQueryHandler<FetchProductsByHandlesQueryHandler>()
                .OverrideFetchProductsByIdsQueryHandler<FetchProductsByIdsQueryHandler>()
                .OverrideFetchProductsBySearchQueryHandler<FetchProductsBySearchQueryHandler>();

            return serviceComponent;
        }
    }
}
