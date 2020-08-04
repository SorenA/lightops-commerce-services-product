using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public interface IProductServiceComponent
    {
        #region Services
        IProductServiceComponent OverrideHealthService<T>() where T : IHealthService;
        IProductServiceComponent OverrideProductService<T>() where T : IProductService;
        #endregion Services

        #region Mappers
        IProductServiceComponent OverrideProductProtoMapper<T>() where T : IMapper<IProduct, ProductProto>;
        IProductServiceComponent OverrideProductVariantProtoMapper<T>() where T : IMapper<IProductVariant, ProductVariantProto>;
        IProductServiceComponent OverrideImageProtoMapper<T>() where T : IMapper<IImage, ImageProto>;
        IProductServiceComponent OverrideMoneyProtoMapper<T>() where T : IMapper<Money, MoneyProto>;
        #endregion Mappers

        #region Query Handlers
        IProductServiceComponent OverrideCheckProductHealthQueryHandler<T>() where T : ICheckProductHealthQueryHandler;

        IProductServiceComponent OverrideFetchProductsByHandlesQueryHandler<T>() where T : IFetchProductsByHandlesQueryHandler;
        IProductServiceComponent OverrideFetchProductsByIdsQueryHandler<T>() where T : IFetchProductsByIdsQueryHandler;
        IProductServiceComponent OverrideFetchProductsBySearchQueryHandler<T>() where T : IFetchProductsBySearchQueryHandler;
        #endregion Query Handlers
    }
}