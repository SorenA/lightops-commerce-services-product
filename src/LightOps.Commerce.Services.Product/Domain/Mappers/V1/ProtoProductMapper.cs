using System.Linq;
using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Services.Product.Domain.Mappers.V1
{
    public class ProtoProductMapper : IMapper<IProduct, ProtoProduct>
    {
        private readonly IMappingService _mappingService;

        public ProtoProductMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ProtoProduct Map(IProduct source)
        {
            var dest = new ProtoProduct();

            dest.Id = source.Id;
            dest.Handle = source.Handle;
            dest.Url = source.Url;

            dest.Title= source.Title;
            dest.Type = source.Type;
            dest.Description = source.Description;

            dest.SeoTitle = source.SeoTitle;
            dest.SeoDescription = source.SeoDescription;

            dest.PrimaryCategoryId = source.PrimaryCategoryId;
            dest.CategoryIds.AddRange(source.CategoryIds);

            var links = _mappingService
                .Map<IProductVariant, ProtoProductVariant>(source.Variants)
                .ToList();
            dest.Variants.AddRange(links);

            dest.PrimaryImage = source.PrimaryImage;
            dest.Images.AddRange(source.Images);

            return dest;
        }
    }
}