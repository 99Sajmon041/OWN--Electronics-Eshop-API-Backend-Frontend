using FluentValidation;

namespace ElectronicsEshop.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id).GreaterThan(0).WithMessage("ID musí být větší než 0.");
    }
}
