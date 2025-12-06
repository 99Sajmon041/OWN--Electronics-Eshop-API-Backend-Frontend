using FluentValidation;

namespace ElectronicsEshop.Application.Orders.Queries.User;

public sealed class GetOrdersCountForUserQueryValidator : AbstractValidator<GetOrdersCountForUserQuery>
{
    public GetOrdersCountForUserQueryValidator()
    {
        RuleFor(oc => oc.UserId).NotEmpty().WithMessage("Uživatelské ID je povinný údaj.");
    }
}
