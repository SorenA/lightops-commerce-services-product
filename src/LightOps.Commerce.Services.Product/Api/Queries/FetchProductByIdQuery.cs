using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}