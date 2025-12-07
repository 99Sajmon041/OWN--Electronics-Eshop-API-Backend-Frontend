using FluentValidation;

namespace ElectronicsEshop.Application.Role.Queries.GetRole;

public sealed class GetRoleQueryValidator : AbstractValidator<GetRoleQuery>
{
    public GetRoleQueryValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("Role ID je povinný parametr.");
    }
}
