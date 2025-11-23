using AutoMapper;
using ElectronicsEshop.Application.Categories.Commands.CreateCategory;
using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Categories.Mapping;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();

        CreateMap<CreateCategoryCommand, Category>();
    }
}
