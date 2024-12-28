namespace Catalog.API.Products.DeleteProduct;





public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>

{
   public DeleteProductCommandValidator()
   {
      RuleFor(c => c.Id).NotEmpty().WithMessage("Product ID is required");
   }
}