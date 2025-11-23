using FluentValidation;

namespace ElectronicsEshop.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Název kategorie je povinný.")
            .Length(2, 100).WithMessage("Název kategorie musí být v rozmezí 2 - 100 znaků.");
    }
}
