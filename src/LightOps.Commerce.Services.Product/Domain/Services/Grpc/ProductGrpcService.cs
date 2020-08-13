using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using LightOps.Commerce.Proto.Services;
using LightOps.Commerce.Proto.Types;
using LightOps.Commerce.Services.Product.Api.Enums;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.Logging;

namespace LightOps.Commerce.Services.Product.Domain.Services.Grpc
{
    public class ProductGrpcService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ILogger<ProductGrpcService> _logger;
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        public ProductGrpcService(
            ILogger<ProductGrpcService> logger,
            IProductService productService,
            IMappingService mappingService)
        {
            _logger = logger;
            _productService = productService;
            _mappingService = mappingService;
        }
        
        public override async Task<GetProductsByHandlesProtoResponse> GetProductsByHandles(GetProductsByHandlesProtoRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetByHandleAsync(request.Handles);
            var protoEntities = _mappingService.Map<IProduct, ProductProto>(entities);

            var result = new GetProductsByHandlesProtoResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }

        public override async Task<GetProductsByIdsProtoResponse> GetProductsByIds(GetProductsByIdsProtoRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetByIdAsync(request.Ids);
            var protoEntities = _mappingService.Map<IProduct, ProductProto>(entities);

            var result = new GetProductsByIdsProtoResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }

        public override async Task<GetProductsBySearchProtoResponse> GetProductsBySearch(GetProductsBySearchProtoRequest request, ServerCallContext context)
        {
            var searchResult = await _productService.GetBySearchAsync(
                request.SearchTerm,
                request.CategoryId,
                request.PageCursor,
                request.PageSize ?? 24,
                ConvertSortKey(request.SortKey),
                request.Reverse ?? false);

            // Map results
            var protoEntities = searchResult
                .Results
                .Select(x => new GetProductsBySearchProtoResponse.Types.ProductResult
                {
                    Cursor = x.Cursor,
                    Node = _mappingService.Map<IProduct, ProductProto>(x.Node),
                })
                .ToList();

            var result = new GetProductsBySearchProtoResponse
            {
                HasNextPage = searchResult.HasNextPage,
                HasPreviousPage = searchResult.HasPreviousPage,
                StartCursor = searchResult.StartCursor ?? string.Empty,
                EndCursor = searchResult.EndCursor ?? string.Empty,
                TotalResults = searchResult.TotalResults,
            };
            result.Results.AddRange(protoEntities);

            return result;
        }

        private ProductSortKey ConvertSortKey(ProductSortKeyProto sortKey)
        {
            switch (sortKey)
            {
                case ProductSortKeyProto.Default:
                    return ProductSortKey.Default;
                case ProductSortKeyProto.Title:
                    return ProductSortKey.Title;
                case ProductSortKeyProto.CreatedAt:
                    return ProductSortKey.CreatedAt;
                case ProductSortKeyProto.UpdatedAt:
                    return ProductSortKey.UpdatedAt;
                case ProductSortKeyProto.UnitPrice:
                    return ProductSortKey.UnitPrice;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
