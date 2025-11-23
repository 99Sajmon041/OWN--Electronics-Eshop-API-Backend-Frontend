using FluentValidation;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrder;

public sealed class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(o => o.Id)
            .GreaterThan(0)
            .WithMessage("ID musí být větší než 0.");
    }
}
