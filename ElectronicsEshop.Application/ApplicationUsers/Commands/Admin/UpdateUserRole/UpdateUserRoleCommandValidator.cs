using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.UpdateUserRole;

public sealed class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty();

        RuleFor(u => u.Role)
            .NotEmpty()
            .WithMessage("Role je povinný údaj.");
    }
}
