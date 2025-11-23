using MediatR;

namespace ElectronicsEshop.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
}
