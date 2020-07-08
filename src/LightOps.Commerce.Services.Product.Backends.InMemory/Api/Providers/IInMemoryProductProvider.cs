using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers
{
    public interface IInMemoryProductProvider
    {
        IList<IProduct> Products { get; }
    }
}