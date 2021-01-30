﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.QueryHandlers
{
    public class FetchProductsByIdsQueryHandler : IFetchProductsByIdsQueryHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public FetchProductsByIdsQueryHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task<IList<Proto.Types.Product>> HandleAsync(FetchProductsByIdsQuery query)
        {
            var products = _inMemoryProductProvider
                .Products?
                .Where(c => query.Ids.Contains(c.Id))
                .ToList();

            return Task.FromResult<IList<Proto.Types.Product>>(products ?? new List<Proto.Types.Product>());
        }
    }
}