using System.Linq;
using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Services.Product.Domain.Mappers
{
    public class ProductProtoMapper : IMapper<IProduct, ProductProto>
    {
        private readonly IMappingService _mappingService;

        public ProductProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ProductProto Map(IProduct src)
        {
            var dest = new ProductProto
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Handle = src.Handle,
                Title = src.Title,
                Url = src.Url,
                Type = src.Type,
                Description = src.Description,
                CreatedAt = Timestamp.FromDateTime(src.CreatedAt.ToUniversalTime()),
                UpdatedAt = Timestamp.FromDateTime(src.UpdatedAt.ToUniversalTime()),
                PrimaryCategoryId = src.PrimaryCategoryId,
            };

            // Add category ids
            dest.CategoryIds.AddRange(src.CategoryIds);

            // Map variants
            var variants = _mappingService
                .Map<IProductVariant, ProductVariantProto>(src.Variants)
                .ToList();
            dest.Variants.AddRange(variants);

            // Map images
            var images = _mappingService
                .Map<IImage, ImageProto>(src.Images)
                .ToList();
            dest.Images.AddRange(images);

            return dest;
        }
    }
}