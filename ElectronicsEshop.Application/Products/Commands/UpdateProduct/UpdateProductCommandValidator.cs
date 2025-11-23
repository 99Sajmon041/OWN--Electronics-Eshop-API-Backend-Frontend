using ElectronicsEshop.Application.Products.Shared.Validators;
using FluentValidation;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID musí být větší než 0.");

        RuleFor(p => p.Data)
            .SetValidator(new ProductUpsertValidator());
    }
}
