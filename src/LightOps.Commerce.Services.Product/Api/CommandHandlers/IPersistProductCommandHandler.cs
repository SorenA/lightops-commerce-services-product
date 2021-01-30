using LightOps.Commerce.Services.Product.Api.Commands;
using LightOps.CQRS.Api.Commands;

namespace LightOps.Commerce.Services.Product.Api.CommandHandlers
{
    public interface IPersistProductCommandHandler : ICommandHandler<PersistProductCommand>
    {
        
    }
}