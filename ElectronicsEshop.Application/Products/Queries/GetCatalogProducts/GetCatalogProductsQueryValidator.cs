using ElectronicsEshop.Application.Common.Enums;
using FluentValidation;

namespace ElectronicsEshop.Application.Products.Queries.GetCatalogProducts;

public sealed class GetCatalogProductsQueryValidator : AbstractValidator<GetCatalogProductsQuery>
{
    public GetCatalogProductsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize);
        RuleFor(x => x.Q).MaximumLength(200);
    }
}
