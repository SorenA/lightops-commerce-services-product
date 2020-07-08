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
        IProductServiceComponent OverrideProtoMoneyMapperV1<T>() where T : IMapper<Money, Proto.Services.Product.V1.ProtoMoney>;
        IProductServiceComponent OverrideProtoProductMapperV1<T>() where T : IMapper<IProduct, Proto.Services.Product.V1.ProtoProduct>;
        IProductServiceComponent OverrideProtoProductVariantMapperV1<T>() where T : IMapper<IProductVariant, Proto.Services.Product.V1.ProtoProductVariant>;
        #endregion Mappers

        #region Query Handlers
        IProductServiceComponent OverrideCheckProductHealthQueryHandler<T>() where T : ICheckProductHealthQueryHandler;
        IProductServiceComponent OverrideFetchProductByHandleQueryHandler<T>() where T : IFetchProductByHandleQueryHandler;
        IProductServiceComponent OverrideFetchProductByIdQueryHandler<T>() where T : IFetchProductByIdQueryHandler;
        IProductServiceComponent OverrideFetchProductsByCategoryIdQueryHandler<T>() where T : IFetchProductsByCategoryIdQueryHandler;
        IProductServiceComponent OverrideFetchProductsBySearchQueryHandler<T>() where T : IFetchProductsBySearchQueryHandler;
        #endregion Query Handlers
    }
}