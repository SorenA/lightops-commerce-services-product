using System.Linq;
using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Mappers
{
    public class ProductVariantProtoMapper : IMapper<IProductVariant, ProductVariantProto>
    {
        private readonly IMappingService _mappingService;

        public ProductVariantProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ProductVariantProto Map(IProductVariant src)
        {
            var dest = new ProductVariantProto
            {
                Id = src.Id,
                ProductId = src.ProductId,
                Title = src.Title,
                Sku = src.Sku,
                UnitPrice = _mappingService.Map<Money, MoneyProto>(src.UnitPrice),
            };

            // Map images
            var images = _mappingService
                .Map<IImage, ImageProto>(src.Images)
                .ToList();
            dest.Images.AddRange(images);

            return dest;
        }
    }
}