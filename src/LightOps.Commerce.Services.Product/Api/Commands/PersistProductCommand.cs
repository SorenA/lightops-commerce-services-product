using LightOps.CQRS.Api.Commands;

namespace LightOps.Commerce.Services.Product.Api.Commands
{
    public class PersistProductCommand : ICommand
    {
        /// <summary>
        /// The id of the product to persist
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The product to persist
        /// </summary>
        public Proto.Types.Product Product { get; set; }
    }
}