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

Product is implemented in `Domain.GrpcServices.ProductGrpcService`.

Health is implemented in `Domain.GrpcServices.HealthGrpcService`.

### Health-check

Health-checks conforms to the [GRPC Health Checking Protocol](https://github.com/grpc/grpc/blob/master/doc/health-checking.md)

Available services are as follows

```bash
service = '' - System as a whole
service = 'lightops.service.ProductService' - Product
```

For embedding a gRPC client for use with Kubernetes, see [grpc-ecosystem/grpc-health-probe](https://github.com/grpc-ecosystem/grpc-health-probe)

## Samples

A sample application hosting the gRPC service with mock data is available in the `samples/Sample.ProductService` project.

## Requirements

LightOps packages available on NuGet:

- `LightOps.DependencyInjection`
- `LightOps.CQRS`

## Using the service component

Register during startup through the `AddProductService(options)` extension on `IDependencyInjectionRootComponent`.

```csharp
services.AddLightOpsDependencyInjection(root =>
{
    root
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

The gRPC services use `ICommandDispatcher` & `IQueryDispatcher` from the `LightOps.CQRS` package to dispatch commands and queries, see configuration bellow.

### Configuration options

A component backend is required, implementing the command & query handlers tied to a data-source, see configuration overridables bellow.

A custom backend, or one of the following standard backends can be used:

- InMemory

### Overridables

Using the `IProductServiceComponent` configuration, the following can be overridden:

```csharp
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
```

## Backend modules

### In-Memory

Register during startup through the `UseInMemoryBackend(root, options)` extension on `IProductServiceComponent`.

```csharp
root.AddProductService(service =>
{
    service.UseInMemoryBackend(root, backend =>
    {
        var products = new List<Product>();
        // ...

        backend.UseProducts(products);
    });

    // Configure service
    // ...
});
```
