using FluentValidation;

namespace ElectronicsEshop.Application.Categories.Queries.GetCategory;

public sealed class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(c => c.Id).GreaterThan(0).WithMessage("ID musí být větší než 0.");
    }
}
