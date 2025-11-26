using FluentValidation;

namespace ElectronicsEshop.Application.Carts.Commands.DeleteCartItem;

public sealed class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
{
    public DeleteCartItemCommandValidator()
    {
        RuleFor(ci => ci.Id).GreaterThan(0).WithMessage("ID položky musí být větší než 0.");
    }
}
