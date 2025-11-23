using MediatR;

namespace ElectronicsEshop.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
