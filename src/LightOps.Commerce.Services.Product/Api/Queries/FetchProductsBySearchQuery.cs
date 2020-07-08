using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsBySearchQuery : IQuery
    {
        public string SearchTerm { get; set; }
    }
}