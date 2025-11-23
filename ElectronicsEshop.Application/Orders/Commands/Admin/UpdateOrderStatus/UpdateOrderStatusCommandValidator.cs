using FluentValidation;

namespace ElectronicsEshop.Application.Orders.Commands.Admin.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(o => o.Id)
            .GreaterThan(0)
            .WithMessage("ID nesmí nabývat záporných hodnot.");

        RuleFor(o => o.NewStatus)
            .IsInEnum()
            .WithMessage("Neplatný stav objednávky.");
    }
}
