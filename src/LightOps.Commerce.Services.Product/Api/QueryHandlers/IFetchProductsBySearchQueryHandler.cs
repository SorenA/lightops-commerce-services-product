using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.QueryHandlers
{
    public interface IFetchProductsBySearchQueryHandler : IQueryHandler<FetchProductsBySearchQuery, SearchResult<Proto.Types.Product>>
    {

    }
}