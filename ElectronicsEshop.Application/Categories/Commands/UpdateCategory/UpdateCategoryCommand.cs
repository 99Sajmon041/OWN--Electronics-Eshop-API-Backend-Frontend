using MediatR;

namespace ElectronicsEshop.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
