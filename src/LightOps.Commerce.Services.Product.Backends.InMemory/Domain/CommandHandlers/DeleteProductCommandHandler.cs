using System;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.CommandHandlers;
using LightOps.Commerce.Services.Product.Api.Commands;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.CommandHandlers
{
    public class DeleteProductCommandHandler : IDeleteProductCommandHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public DeleteProductCommandHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task HandleAsync(DeleteProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
            {
                throw new ArgumentException("ID missing.");
            }

            // Check if entity already exists
            var entity = _inMemoryProductProvider
                .Products?
                .FirstOrDefault(x => x.Id == command.Id);

            // Delete old if found
            if (entity != null)
            {
                _inMemoryProductProvider.Products?.Remove(entity);
            }

            return Task.CompletedTask;
        }
    }
}