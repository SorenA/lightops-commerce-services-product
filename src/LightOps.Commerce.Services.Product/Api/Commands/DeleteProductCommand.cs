using LightOps.CQRS.Api.Commands;

namespace LightOps.Commerce.Services.Product.Api.Commands
{
    public class DeleteProductCommand : ICommand
    {
        /// <summary>
        /// The id of the product to delete
        /// </summary>
        public string Id { get; set; }
    }
}