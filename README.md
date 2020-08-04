# LightOps Commerce - Product Service

Microservice for products.

Defines products and product variants.  
Uses CQRS to fetch entities from data-source without defining any.  
Provides gRPC services for integrations into other services.

![Nuget](https://img.shields.io/nuget/v/LightOps.Commerce.Services.Product)

| Branch | CI |
| --- | --- |
| master | ![Build Status](https://dev.azure.com/sorendev/LightOps%20Commerce/_apis/build/status/LightOps.Commerce.Services.Product?branchName=master) |
| develop | ![Build Status](https://dev.azure.com/sorendev/LightOps%20Commerce/_apis/build/status/LightOps.Commerce.Services.Product?branchName=develop) |

## gRPC services

Protobuf service definitions located at [SorenA/lightops-commerce-proto](https://github.com/SorenA/lightops-commerce-proto).

Product is implemented in `Domain.Services.Grpc.ProductGrpcService`.

Health is implemented in `Domain.Services.Grpc.HealthGrpcService`.

### Health-check

Health-checks conforms to the [GRPC Health Checking Protocol](https://github.com/grpc/grpc/blob/master/doc/health-checking.md)

Available services are as follows

```bash
service = '' - System as a whole
service = 'lightops.service.ProductProtoService' - Product
```

For embedding a gRPC client for use with Kubernetes, see [grpc-ecosystem/grpc-health-probe](https://github.com/grpc-ecosystem/grpc-health-probe)

## Samples

A sample application hosting the gRPC service with mock data is available in the `samples/Sample.ProductService` project.

## Requirements

LightOps packages available on NuGet:

- `LightOps.DependencyInjection`
- `LightOps.CQRS`
- `LightOps.Mapping`

## Using the service component

Register during startup through the `AddProductService(options)` extension on `IDependencyInjectionRootComponent`.

```csharp
services.AddLightOpsDependencyInjection(root =>
{
    root
        .AddMapping()
        .AddCqrs()
        .AddProductService(service =>
        {
            // Configure service
            // ...
        });
});

services.AddGrpc();
```

Register gRPC services for integrations.

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ProductGrpcService>();
    endpoints.MapGrpcService<HealthGrpcService>();

    // Map other endpoints...
});
```

### Configuration options

A component backend is required, defining the query handlers tied to a data-source, see **Query handlers** section bellow for more.

A custom backend, or one of the following standard backends can be used:

- InMemory

### Overridables

Using the `IProductServiceComponent` configuration, the following can be overridden:

```csharp
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
```

`IProductService` is used by the gRPC services and query the data using the `IQueryDispatcher` from the `LightOps.CQRS` package.

The mappers are used for mapping the internal data structure to the versioned protobuf messages.

## Backend modules

### In-Memory

Register during startup through the `UseInMemoryBackend(root, options)` extension on `IProductServiceComponent`.

```csharp
root.AddProductService(service =>
{
    service.UseInMemoryBackend(root, backend =>
    {
        var products = new List<IProduct>();
        // ...

        backend.UseProducts(products);
    });

    // Configure service
    // ...
});
```
