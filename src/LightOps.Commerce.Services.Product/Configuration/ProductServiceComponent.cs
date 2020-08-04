using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Commerce.Services.Product.Domain.Mappers;
using LightOps.Commerce.Services.Product.Domain.Services;
using LightOps.CQRS.Api.Queries;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;
using LightOps.Mapping.Api.Mappers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public class ProductServiceComponent : IDependencyInjectionComponent, IProductServiceComponent
    {
        public string Name => "lightops.commerce.services.product";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .Union(_services.Values)
                .Union(_mappers.Values)
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

        #region Mappers
        internal enum Mappers
        {
            ProductProtoMapper,
            ProductVariantProtoMapper,
            ImageProtoMapper,
            MoneyProtoMapper,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ProductProtoMapper] = ServiceRegistration.Transient<IMapper<IProduct, ProductProto>, ProductProtoMapper>(),
            [Mappers.ProductVariantProtoMapper] = ServiceRegistration.Transient<IMapper<IProductVariant, ProductVariantProto>, ProductVariantProtoMapper>(),
            [Mappers.ImageProtoMapper] = ServiceRegistration.Transient<IMapper<IImage, ImageProto>, ImageProtoMapper>(),
            [Mappers.MoneyProtoMapper] = ServiceRegistration.Transient<IMapper<Money, MoneyProto>, MoneyProtoMapper>(),
        };

        public IProductServiceComponent OverrideProductProtoMapper<T>() where T : IMapper<IProduct, ProductProto>
        {
            _mappers[Mappers.ProductProtoMapper].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideProductVariantProtoMapper<T>() where T : IMapper<IProductVariant, ProductVariantProto>
        {
            _mappers[Mappers.ProductVariantProtoMapper].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideImageProtoMapper<T>() where T : IMapper<IImage, ImageProto>
        {
            _mappers[Mappers.ImageProtoMapper].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideMoneyProtoMapper<T>() where T : IMapper<Money, MoneyProto>
        {
            _mappers[Mappers.MoneyProtoMapper].ImplementationType = typeof(T);
            return this;
        }
        #endregion Mappers

        #region Query Handlers
        internal enum QueryHandlers
        {
            CheckProductHealthQueryHandler,

            FetchProductsByHandlesQueryHandler,
            FetchProductsByIdsQueryHandler,
            FetchProductsBySearchQueryHandler,
        }

        private readonly Dictionary<QueryHandlers, ServiceRegistration> _queryHandlers = new Dictionary<QueryHandlers, ServiceRegistration>
        {
            [QueryHandlers.CheckProductHealthQueryHandler] = ServiceRegistration.Transient<IQueryHandler<CheckProductHealthQuery, HealthStatus>>(),

            [QueryHandlers.FetchProductsByHandlesQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsByHandlesQuery, IList<IProduct>>>(),
            [QueryHandlers.FetchProductsByIdsQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsByIdsQuery, IList<IProduct>>>(),
            [QueryHandlers.FetchProductsBySearchQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsBySearchQuery, SearchResult<IProduct>>>(),
        };

        public IProductServiceComponent OverrideCheckProductHealthQueryHandler<T>() where T : ICheckProductHealthQueryHandler
        {
            _queryHandlers[QueryHandlers.CheckProductHealthQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductsByHandlesQueryHandler<T>() where T : IFetchProductsByHandlesQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductsByHandlesQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideFetchProductsByIdsQueryHandler<T>() where T : IFetchProductsByIdsQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchProductsByIdsQueryHandler].ImplementationType = typeof(T);
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