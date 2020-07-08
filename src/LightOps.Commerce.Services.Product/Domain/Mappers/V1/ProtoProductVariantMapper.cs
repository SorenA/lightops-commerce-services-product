using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Mappers.V1
{
    public class ProtoProductVariantMapper : IMapper<IProductVariant, ProtoProductVariant>
    {
        private readonly IMappingService _mappingService;

        public ProtoProductVariantMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ProtoProductVariant Map(IProductVariant source)
        {
            var dest = new ProtoProductVariant();

            dest.Id = source.Id;

            dest.ProductId = source.ProductId;

            dest.Title = source.Title;
            dest.Sku = source.Sku;

            dest.Money = _mappingService
                .Map<Money, ProtoMoney>(source.Price);

            return dest;
        }
    }
}