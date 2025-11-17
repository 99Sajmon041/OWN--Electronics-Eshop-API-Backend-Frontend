using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty();
    }
}
