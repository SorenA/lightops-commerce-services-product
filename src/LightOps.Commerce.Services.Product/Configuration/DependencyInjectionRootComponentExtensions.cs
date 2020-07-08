using System;
using LightOps.DependencyInjection.Configuration;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public static class DependencyInjectionRootComponentExtensions
    {

        public static IDependencyInjectionRootComponent AddProductService(this IDependencyInjectionRootComponent rootComponent, Action<IProductServiceComponent> componentConfig = null)
        {
            var component = new ProductServiceComponent();

            // Invoke config delegate
            componentConfig?.Invoke(component);

            // Attach to root component
            rootComponent.AttachComponent(component);

            return rootComponent;
        }
    }
}
