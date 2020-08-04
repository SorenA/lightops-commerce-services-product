using System.Collections.Generic;
using LightOps.CQRS.Api.Queries;

namespace LightOps.Commerce.Services.Product.Api.Queries
{
    public class FetchProductsByHandlesQuery : IQuery
    {
        /// <summary>
        /// The handles of the products requested
        /// </summary>
        public IList<string> Handles { get; set; }
    }
}