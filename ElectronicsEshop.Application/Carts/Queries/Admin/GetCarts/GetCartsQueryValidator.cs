using ElectronicsEshop.Application.Common.Enums;
using FluentValidation;

namespace ElectronicsEshop.Application.Carts.Queries.Admin.GetCarts;

public sealed class GetCartsQueryValidator : AbstractValidator<GetCartsQuery>
{
    public GetCartsQueryValidator()
    {
        RuleFor(c => c.Page).GreaterThanOrEqualTo(1).WithMessage("Strana musí být větší / rovno 1");
        RuleFor(x => x.PageSize).InclusiveBetween(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize);
    }
}
 