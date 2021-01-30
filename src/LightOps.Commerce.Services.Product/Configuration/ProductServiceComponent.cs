using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Services.Product.Api.CommandHandlers;
using LightOps.Commerce.Services.Product.Api.Commands;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryHandlers;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.CQRS.Api.Commands;
using LightOps.CQRS.Api.Queries;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.Product.Configuration
{
    public class ProductServiceComponent : IDependencyInjectionComponent, IProductServiceComponent
    {
        public string Name => "lightops.commerce.services.product";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .Union(_queryHandlers.Values)
                .Union(_commandHandlers.Values)
                .ToList();
        }

        #region Query Handlers
        internal enum QueryHandlers
        {
            CheckProductServiceHealthQueryHandler,

            FetchProductsByHandlesQueryHandler,
            FetchProductsByIdsQueryHandler,
            FetchProductsBySearchQueryHandler,
        }

        private readonly Dictionary<QueryHandlers, ServiceRegistration> _queryHandlers = new Dictionary<QueryHandlers, ServiceRegistration>
        {
            [QueryHandlers.CheckProductServiceHealthQueryHandler] = ServiceRegistration.Transient<IQueryHandler<CheckProductServiceHealthQuery, HealthStatus>>(),

            [QueryHandlers.FetchProductsByHandlesQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsByHandlesQuery, IList<Proto.Types.Product>>>(),
            [QueryHandlers.FetchProductsByIdsQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsByIdsQuery, IList<Proto.Types.Product>>>(),
            [QueryHandlers.FetchProductsBySearchQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchProductsBySearchQuery, SearchResult<Proto.Types.Product>>>(),
        };

        public IProductServiceComponent OverrideCheckProductServiceHealthQueryHandler<T>() where T : ICheckProductServiceHealthQueryHandler
        {
            _queryHandlers[QueryHandlers.CheckProductServiceHealthQueryHandler].ImplementationType = typeof(T);
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

        #region Command Handlers
        internal enum CommandHandlers
        {
            PersistProductCommandHandler,
            DeleteProductCommandHandler,
        }

        private readonly Dictionary<CommandHandlers, ServiceRegistration> _commandHandlers = new Dictionary<CommandHandlers, ServiceRegistration>
        {
            [CommandHandlers.PersistProductCommandHandler] = ServiceRegistration.Transient<ICommandHandler<PersistProductCommand>>(),
            [CommandHandlers.DeleteProductCommandHandler] = ServiceRegistration.Transient<ICommandHandler<DeleteProductCommand>>(),
        };

        public IProductServiceComponent OverridePersistProductCommandHandler<T>() where T : IPersistProductCommandHandler
        {
            _commandHandlers[CommandHandlers.PersistProductCommandHandler].ImplementationType = typeof(T);
            return this;
        }

        public IProductServiceComponent OverrideDeleteProductCommandHandler<T>() where T : IDeleteProductCommandHandler
        {
            _commandHandlers[CommandHandlers.DeleteProductCommandHandler].ImplementationType = typeof(T);
            return this;
        }
        #endregion Command Handlers
    }
}