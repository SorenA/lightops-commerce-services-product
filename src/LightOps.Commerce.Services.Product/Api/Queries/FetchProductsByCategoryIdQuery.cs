using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsByCategoryIdQuery : IQuery
    {
        public string CategoryId { get; set; }
    }
}