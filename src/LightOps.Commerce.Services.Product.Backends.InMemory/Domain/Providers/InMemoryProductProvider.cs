﻿using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.Providers
{
    public class InMemoryProductProvider : IInMemoryProductProvider
    {
        public IList<Proto.Types.Product> Products { get; internal set; } = new List<Proto.Types.Product>();
    }
}