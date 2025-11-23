using FluentValidation;

namespace ElectronicsEshop.Application.Products.Queries.GetProduct;

public sealed class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("ID musí být větší než 0.");
    }
}
