using LightOps.Commerce.Services.Product.Api.CommandHandlers;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public interface IProductServiceComponent
    {
        #region Query Handlers
        IProductServiceComponent OverrideCheckProductServiceHealthQueryHandler<T>() where T : ICheckProductServiceHealthQueryHandler;

        IProductServiceComponent OverrideFetchProductsByHandlesQueryHandler<T>() where T : IFetchProductsByHandlesQueryHandler;
        IProductServiceComponent OverrideFetchProductsByIdsQueryHandler<T>() where T : IFetchProductsByIdsQueryHandler;
        IProductServiceComponent OverrideFetchProductsBySearchQueryHandler<T>() where T : IFetchProductsBySearchQueryHandler;
        #endregion Query Handlers

        #region Command Handlers
        IProductServiceComponent OverridePersistProductCommandHandler<T>() where T : IPersistProductCommandHandler;
        IProductServiceComponent OverrideDeleteProductCommandHandler<T>() where T : IDeleteProductCommandHandler;
        #endregion Command Handlers
    }
}