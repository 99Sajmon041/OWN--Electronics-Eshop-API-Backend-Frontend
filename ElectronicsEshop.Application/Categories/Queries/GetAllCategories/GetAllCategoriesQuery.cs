using ElectronicsEshop.Application.Categories.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>
{
}
