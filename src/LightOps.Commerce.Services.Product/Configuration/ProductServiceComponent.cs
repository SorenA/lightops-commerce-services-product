using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Commerce.Services.Product.Domain.Services;
using LightOps.CQRS.Api.Queries;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;
using LightOps.Mapping.Api.Mappers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public class ProductServiceComponent : IDependencyInjectionComponent, IProductServiceComponent
    {
        public string Name => "lightops.commerce.services.product";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .Union(_services.Values)
                .Union(_queryHandlers.Values)
                .ToList();
        }

        #region Services
        internal enum Services
        {
            HealthService,
            ProductService,
        }

        private readonly Dictionary<Services, ServiceRegistration> _services = new Dictionary<Services, ServiceRegistration>
        {
            [Services.HealthService] = ServiceRegistration.Transient<IHealthService, HealthService>(),
            [Services.ProductService] = ServiceRegistration.Scoped<IProductService, ProductService>(),
        };

        public IProductServiceComponent OverrideHealthService<T>()
            where T : IHealthService
        {
            _services[Services.HealthService].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideProductService<T>()
            where T : IProductService
        {
            _services[Services.ProductService].ImplementationType = typeof(T);
            return this;
        }
        #endregion Services

        #region Query Handlers
        internal enum QueryHandlers
        {
            CheckProductHealthQueryHandler,
            FetchProductByHandleQueryHandler,
            FetchProductByIdQueryHandler,
            FetchProductsByCategoryIdQueryHandler,
            FetchProductsBySearchQueryHandler,
        }

        private readonly Dictionary<QueryHandlers, ServiceRegistration> _queryHandlers = new Dictionary<QueryHandlers, ServiceRegistration>
        {
            [QueryHandlers.CheckProductHealthQueryHandler] = ServiceRegistration.Transient<IQueryHandler<CheckProductHealthQuery, HealthStatus>>(),
            [QueryHandlers.FetchProductByHandleQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductByHandleQuery, IProduct>>(),
            [QueryHandlers.FetchProductByIdQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductByIdQuery, IProduct>>(),
            [QueryHandlers.FetchProductsByCategoryIdQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsByCategoryIdQuery, IList<IProduct>>>(),
            [QueryHandlers.FetchProductsBySearchQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsBySearchQuery, IList<IProduct>>>(),
        };

        public IProductServiceComponent OverrideCheckProductHealthQueryHandler<T>() where T : ICheckProductHealthQueryHandler
        {
            _queryHandlers[QueryHandlers.CheckProductHealthQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductByHandleQueryHandler<T>() where T : IFetchProductByHandleQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductByHandleQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductByIdQueryHandler<T>() where T : IFetchProductByIdQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductByIdQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductsByCategoryIdQueryHandler<T>() where T : IFetchProductsByCategoryIdQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductsByCategoryIdQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductsBySearchQueryHandler<T>() where T : IFetchProductsBySearchQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductsBySearchQueryHandler].ImplementationType = typeof(T);
            return this;
        }
        #endregion Query Handlers
    }
}