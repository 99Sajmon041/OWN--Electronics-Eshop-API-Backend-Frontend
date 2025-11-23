using AutoMapper;
using ElectronicsEshop.Application.Products.DTOs;
using ElectronicsEshop.Application.Products.Shared.DTOs;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Products.Mapping;

public sealed class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductListItemDto>()
            .ForMember(d => d.CategoryName, m => m.MapFrom(p => p.Category.Name))
            .ForMember(d => d.FinalPrice, m => m.MapFrom(p => CalcFinalPrice(p.Price, p.DiscountPercentage)));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(d => d.CategoryName, m => m.MapFrom(p => p.Category.Name))
            .ForMember(d => d.FinalPrice, m => m.MapFrom(p => CalcFinalPrice(p.Price, p.DiscountPercentage)));

        CreateMap<ProductUpsertDto, Product>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }

    private static decimal CalcFinalPrice(decimal price, decimal discountPercent)
        => price * (1 - (discountPercent / 100m));
}
