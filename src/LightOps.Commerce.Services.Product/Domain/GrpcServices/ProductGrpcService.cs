using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using LightOps.Commerce.Proto.Services;
using LightOps.Commerce.Services.Product.Api.Commands;
using LightOps.Commerce.Services.Product.Api.Queries;
using LightOps.Commerce.Services.Product.Api.QueryResults;
using LightOps.CQRS.Api.Services;
using Microsoft.Extensions.Logging;

namespace LightOps.Commerce.Services.Product.Domain.GrpcServices
{
    public class ProductGrpcService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ILogger<ProductGrpcService> _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ProductGrpcService(
            ILogger<ProductGrpcService> logger,
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<PersistResponse> Persist(PersistRequest request, ServerCallContext context)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new PersistProductCommand
                {
                    Id = request.Id,
                    Product = request.Product,
                });

                return new PersistResponse
                {
                    StatusCode = PersistResponse.Types.StatusCode.Ok,
                };
            }
            catch (ArgumentException e)
            {
                _logger.LogError($"Failed persisting entity {request.Id}", e);
                return new PersistResponse
                {
                    StatusCode = PersistResponse.Types.StatusCode.Invalid,
                    Errors = { e.Message },
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed persisting entity {request.Id}", e);
            }

            return new PersistResponse
            {
                StatusCode = PersistResponse.Types.StatusCode.Unavailable,
            };
        }

        public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new DeleteProductCommand
                {
                    Id = request.Id,
                });

                return new DeleteResponse
                {
                    StatusCode = DeleteResponse.Types.StatusCode.Ok,
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed deleting entity {request.Id}", e);
            }

            return new DeleteResponse
            {
                StatusCode = DeleteResponse.Types.StatusCode.Unavailable,
            };
        }

        public override async Task<GetByHandlesResponse> GetByHandles(GetByHandlesRequest request, ServerCallContext context)
        {
            try
            {
                var entities = await _queryDispatcher.DispatchAsync<FetchProductsByHandlesQuery, IList<Proto.Types.Product>>(new FetchProductsByHandlesQuery
                {
                    Handles = request.Handles,
                });

                return new GetByHandlesResponse
                {
                    Products = { entities },
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed fetching by handles '{string.Join(",", request.Handles)}'", e);
            }

            return new GetByHandlesResponse();
        }

        public override async Task<GetByIdsResponse> GetByIds(GetByIdsRequest request, ServerCallContext context)
        {
            try
            {
                var entities = await _queryDispatcher.DispatchAsync<FetchProductsByIdsQuery, IList<Proto.Types.Product>>(new FetchProductsByIdsQuery
                {
                    Ids = request.Ids,
                });

                return new GetByIdsResponse
                {
                    Products = { entities },
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed fetching by ids '{string.Join(",", request.Ids)}'", e);
            }

            return new GetByIdsResponse();
        }

        public override async Task<GetBySearchResponse> GetBySearch(GetBySearchRequest request, ServerCallContext context)
        {
            try
            {
                var searchResult = await _queryDispatcher.DispatchAsync<FetchProductsBySearchQuery, SearchResult<Proto.Types.Product>>(new FetchProductsBySearchQuery
                {
                    SearchTerm = request.SearchTerm,
                    LanguageCode = request.LanguageCode,
                    CategoryId = request.CategoryId,
                    PageCursor = request.PageCursor,
                    PageSize = request.PageSize ?? 24,
                    SortKey = request.SortKey,
                    Reverse = request.Reverse ?? false,
                    CurrencyCode = request.CurrencyCode,
                });

                return new GetBySearchResponse
                {
                    Results =
                    {
                        searchResult.Results.Select(x => new GetBySearchResponse.Types.Result()
                        {
                            Cursor = x.Cursor,
                            Node = x.Node
                        })
                    },
                    HasNextPage = searchResult.HasNextPage,
                    HasPreviousPage = searchResult.HasPreviousPage,
                    StartCursor = searchResult.StartCursor ?? string.Empty,
                    EndCursor = searchResult.EndCursor ?? string.Empty,
                    TotalResults = searchResult.TotalResults,
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed fetching by search '{JsonSerializer.Serialize(request)}'", e);
            }

            return new GetBySearchResponse();
        }
    }
}
