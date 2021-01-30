using System.Collections.Generic;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers
{
    public interface IInMemoryProductProvider
    {
        IList<Proto.Types.Product> Products { get; }
    }
}