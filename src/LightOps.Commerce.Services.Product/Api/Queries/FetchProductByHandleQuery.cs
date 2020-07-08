using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductByHandleQuery : IQuery
    {
        public string Handle { get; set; }
    }
}