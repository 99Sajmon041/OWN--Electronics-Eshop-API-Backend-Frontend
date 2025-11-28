using ElectronicsEshop.Application.Common.Enums;
using FluentValidation;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrders;

public sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Strana musí být větší nebo rovna 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize);

        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .When(x => x.OrderId.HasValue);

        RuleFor(x => x.To)
            .Must((q, to) => !to.HasValue || !q.From.HasValue || to >= q.From)
            .WithMessage("Datum 'Do' musí být větší nebo rovno datu 'Od'.");
    }
}
