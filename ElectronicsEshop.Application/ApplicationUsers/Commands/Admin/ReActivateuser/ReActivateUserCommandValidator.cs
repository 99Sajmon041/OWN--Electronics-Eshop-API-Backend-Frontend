using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.ReActivateuser;

public sealed class ReActivateUserCommandValidator : AbstractValidator<ReActivateUserCommand>
{
    public ReActivateUserCommandValidator()
    {
        RuleFor(u => u.UserId).NotEmpty().WithMessage("Musíte zadat uživatelské ID.");
    }
}
