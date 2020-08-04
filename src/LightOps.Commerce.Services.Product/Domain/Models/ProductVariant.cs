using System.Collections.Generic;
using LightOps.Commerce.Services.Product.Api.Models;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Models
{
    public class ProductVariant : IProductVariant
    {
        public ProductVariant()
        {
            Images = new List<IImage>();
        }

        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Sku { get; set; }
        public Money UnitPrice { get; set; }
        public IList<IImage> Images { get; set; }
    }
}