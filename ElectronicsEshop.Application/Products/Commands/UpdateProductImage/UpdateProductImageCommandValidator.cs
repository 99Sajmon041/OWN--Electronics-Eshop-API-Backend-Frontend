using FluentValidation;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProductImage;

public sealed class UpdateProductImageCommandValidator : AbstractValidator<UpdateProductImageCommand>
{
    public UpdateProductImageCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("ID produktu musí být větší než 0.");

        RuleFor(c => c.ImageUrl)
            .NotEmpty()
            .WithMessage("ImageUrl je povinná.");
    }
}
