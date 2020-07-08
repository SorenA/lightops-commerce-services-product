using LightOps.Commerce.Services.Product.Api.Models;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Models
{
    public class ProductVariant : IProductVariant
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }
        public string Sku { get; set; }

        public Money Price { get; set; }
    }
}