using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.QueryHandlers
{
    public interface IFetchProductsByIdsQueryHandler : IQueryHandler<FetchProductsByIdsQuery, IList<Proto.Types.Product>>
    {

    }
}