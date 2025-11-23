using FluentValidation;

namespace ElectronicsEshop.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Název kategorie je povinný.")
            .Length(2, 100).WithMessage("Název kategorie musí být v rozmezí 2 - 100 znaků.");
    }
}
