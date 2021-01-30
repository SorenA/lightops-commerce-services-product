using System;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.CommandHandlers;
using LightOps.Commerce.Services.Product.Api.Commands;
using LightOps.Commerce.Services.Product.Backends.InMemory.Api.Providers;

namespace LightOps.Commerce.Services.Product.Backends.InMemory.Domain.CommandHandlers
{
    public class PersistProductCommandHandler : IPersistProductCommandHandler
    {
        private readonly IInMemoryProductProvider _inMemoryProductProvider;

        public PersistProductCommandHandler(IInMemoryProductProvider inMemoryProductProvider)
        {
            _inMemoryProductProvider = inMemoryProductProvider;
        }

        public Task HandleAsync(PersistProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
            {
                throw new ArgumentException("ID missing.");
            }

            if (command.Product.Id != command.Id)
            {
                throw new ArgumentException("Provided ID and entity ID do not match.");
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

            // Add entity to provider
            _inMemoryProductProvider.Products?.Add(command.Product);

            return Task.CompletedTask;
        }
    }
}