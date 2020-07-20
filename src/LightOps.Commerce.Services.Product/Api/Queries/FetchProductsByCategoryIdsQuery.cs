using System.Collections.Generic;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsByCategoryIdsQuery : IQuery
    {
        public IList<string> CategoryIds { get; set; }
    }
}