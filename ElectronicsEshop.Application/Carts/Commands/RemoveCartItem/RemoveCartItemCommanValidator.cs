using FluentValidation;

namespace ElectronicsEshop.Application.Carts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommanValidator : AbstractValidator<RemoveQtyCartItemComman>
{
    public RemoveCartItemCommanValidator()
    {
        RuleFor(ci => ci.ProductId)
            .NotEmpty()
            .WithMessage("Musíte zvolit produkt.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("ID Produktu musí být větší / rovno 1.");

        RuleFor(ci => ci.Quantity)
            .NotEmpty()
            .WithMessage("Musíte zadat množství.")
            .InclusiveBetween(1, 100)
            .WithMessage("Množství musí být v rozmezí 1 - 100.");
    }
}
