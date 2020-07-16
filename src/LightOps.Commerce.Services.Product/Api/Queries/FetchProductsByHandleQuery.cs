using System.Collections.Generic;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsByHandleQuery : IQuery
    {
        public IList<string> Handles { get; set; }
    }
}