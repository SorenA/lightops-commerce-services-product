﻿using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.QueryHandlers
{
    public interface IFetchProductsByCategoryIdQueryHandler : IQueryHandler<FetchProductsByCategoryIdQuery, IList<IProduct>>
    {

    }
}