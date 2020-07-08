using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public interface IProductServiceComponent
    {
        #region Services
        IProductServiceComponent OverrideHealthService<T>() where T : IHealthService;
        IProductServiceComponent OverrideProductService<T>() where T : IProductService;
        #endregion Services

        #region Query Handlers
        IProductServiceComponent OverrideCheckProductHealthQueryHandler<T>() where T : ICheckProductHealthQueryHandler;
        IProductServiceComponent OverrideFetchProductByHandleQueryHandler<T>() where T : IFetchProductByHandleQueryHandler;
        IProductServiceComponent OverrideFetchProductByIdQueryHandler<T>() where T : IFetchProductByIdQueryHandler;
        IProductServiceComponent OverrideFetchProductsByCategoryIdQueryHandler<T>() where T : IFetchProductsByCategoryIdQueryHandler;
        IProductServiceComponent OverrideFetchProductsBySearchQueryHandler<T>() where T : IFetchProductsBySearchQueryHandler;
        #endregion Query Handlers
    }
}