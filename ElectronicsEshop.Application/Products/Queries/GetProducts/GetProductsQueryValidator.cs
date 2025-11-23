using ElectronicsEshop.Application.Common.Enums;
using FluentValidation;

namespace ElectronicsEshop.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    private static readonly string[] AllowedSort = ["name", "price", "stockqty", "productcode"];
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize);
        RuleFor(x => x.Order).IsInEnum();
        RuleFor(x => x.Q).MaximumLength(200);

        RuleFor(x => x.Sort)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(64)
            .Must(s => string.IsNullOrWhiteSpace(s) || AllowedSort.Contains(s.ToLowerInvariant()))
            .WithMessage($"Sort musí být jeden z: {string.Join(", ", AllowedSort)}");

        RuleFor(x => x.PriceMin).GreaterThanOrEqualTo(0).When(x => x.PriceMin.HasValue);
        RuleFor(x => x.PriceMax).GreaterThanOrEqualTo(0).When(x => x.PriceMax.HasValue);
        RuleFor(x => x.PriceMax).GreaterThanOrEqualTo(x => x.PriceMin!.Value)
            .When(x => x.PriceMin.HasValue && x.PriceMax.HasValue);

        RuleFor(x => x.StockMin).GreaterThanOrEqualTo(0).When(x => x.StockMin.HasValue);
        RuleFor(x => x.StockMax).GreaterThanOrEqualTo(0).When(x => x.StockMax.HasValue);
        RuleFor(x => x.StockMax).GreaterThanOrEqualTo(x => x.StockMin!.Value)
            .When(x => x.StockMin.HasValue && x.StockMax.HasValue);
    }
}
