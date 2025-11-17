using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUser;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty();
    }
}
