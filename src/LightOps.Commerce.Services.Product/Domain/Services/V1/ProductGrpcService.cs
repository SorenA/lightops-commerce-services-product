using System.Threading.Tasks;
using Grpc.Core;
using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Api.Services;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.Logging;

namespace LightOps.Commerce.Services.Product.Domain.Services.V1
{
    public class ProductGrpcService : ProtoProductService.ProtoProductServiceBase
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

        public override async Task<ProtoGetProductResponse> GetProduct(ProtoGetProductRequest request, ServerCallContext context)
        {
            IProduct entity;
            switch (request.IdentifierCase)
            {
                case ProtoGetProductRequest.IdentifierOneofCase.Id:
                    entity = await _productService.GetByIdAsync(request.Id);
                    break;
                case ProtoGetProductRequest.IdentifierOneofCase.Handle:
                    entity = await _productService.GetByHandleAsync(request.Handle);
                    break;
                default:
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing identifier"));
            }

            var protoEntity = _mappingService.Map<IProduct, ProtoProduct>(entity);

            var result = new ProtoGetProductResponse
            {
                Product = protoEntity,
            };

            return result;
        }

        public override async Task<GetProductsByIdResponse> GetProductsById(GetProductsByIdRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetByIdAsync(request.Ids);
            var protoEntities = _mappingService.Map<IProduct, ProtoProduct>(entities);

            var result = new GetProductsByIdResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }

        public override async Task<GetProductsByHandleResponse> GetProductsByHandle(GetProductsByHandleRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetByHandleAsync(request.Handles);
            var protoEntities = _mappingService.Map<IProduct, ProtoProduct>(entities);

            var result = new GetProductsByHandleResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }

        public override async Task<ProtoGetProductsByCategoryIdResponse> GetProductsByCategoryId(ProtoGetProductsByCategoryIdRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetByCategoryIdAsync(request.CategoryId);
            var protoEntities = _mappingService.Map<IProduct, ProtoProduct>(entities);

            var result = new ProtoGetProductsByCategoryIdResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }

        public override async Task<ProtoGetProductsBySearchResponse> GetProductsBySearch(ProtoGetProductsBySearchRequest request, ServerCallContext context)
        {
            var entities = await _productService.GetBySearchAsync(request.SearchTerm);
            var protoEntities = _mappingService.Map<IProduct, ProtoProduct>(entities);

            var result = new ProtoGetProductsBySearchResponse();
            result.Products.AddRange(protoEntities);

            return result;
        }
    }
}
