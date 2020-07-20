using System.Collections.Generic;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsByIdsQuery : IQuery
    {
        public IList<string> Ids { get; set; }
    }
}