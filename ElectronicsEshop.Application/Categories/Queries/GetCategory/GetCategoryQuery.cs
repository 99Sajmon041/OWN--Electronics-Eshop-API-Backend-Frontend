using ElectronicsEshop.Application.Categories.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Categories.Queries.GetCategory;

public sealed class GetCategoryQuery(int id) : IRequest<CategoryDto>
{
    public int Id { get; init; } = id;
}
