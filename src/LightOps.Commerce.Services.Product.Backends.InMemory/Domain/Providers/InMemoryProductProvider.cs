using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.Providers
{
    public class InMemoryProductProvider : IInMemoryProductProvider
    {
        public IList<IProduct> Products { get; internal set; } = new List<IProduct>();
    }
}