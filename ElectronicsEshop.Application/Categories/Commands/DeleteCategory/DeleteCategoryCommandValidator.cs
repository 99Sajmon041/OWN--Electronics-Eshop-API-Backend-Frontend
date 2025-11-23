using FluentValidation;

namespace ElectronicsEshop.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(c => c.Id).GreaterThan(0).WithMessage("ID musí být větší než 0.");
    }
}
