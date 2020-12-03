using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Configuration
{
    public interface IInMemoryProductServiceBackendComponent
    {
        #region Entities
        IInMemoryProductServiceBackendComponent UseProducts(IList<IProduct> products);
        #endregion Entities

        #region Providers
        IInMemoryProductServiceBackendComponent OverrideProductProvider<T>() where T : IInMemoryProductProvider;
        #endregion Providers
    }
}