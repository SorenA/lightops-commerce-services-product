using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Configuration
{
    public interface IInMemoryProductServiceBackendComponent
    {
        #region Entities
        IInMemoryProductServiceBackendComponent UseProducts(IList<IProduct> products);
        #endregion Entities
    }
}