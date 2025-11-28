using ElectronicsEshop.Application.Common.Enums;
using FluentValidation;

namespace ElectronicsEshop.Application.Payments.Queries.GetPayments;

public sealed class GetPaymentsQueryValidator : AbstractValidator<GetPaymentsQuery>
{
    public GetPaymentsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
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
