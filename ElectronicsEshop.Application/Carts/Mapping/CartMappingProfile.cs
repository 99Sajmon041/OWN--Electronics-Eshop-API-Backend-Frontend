using AutoMapper;
using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Carts.Mapping;

public sealed class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<CartItem, CartItemDto>()
            .ForMember(c => c.ProductName, opt => opt.MapFrom(ci => ci.Product.Name))
            .ForMember(c => c.Total, opt => opt.MapFrom(ci => ci.Quantity * ci.UnitPrice))
            .ForMember(c => c.ImageUrl, opt => opt.MapFrom(ci => ci.Product.ImageUrl));

        CreateMap<Cart, CartDetailDto>()
            .ForMember(c => c.UserId, opt => opt.MapFrom(ce => ce.ApplicationUserId))
            .ForMember(c => c.UserEmail, opt => opt.MapFrom(ce => ce.ApplicationUser.Email))
            .ForMember(c => c.TotalQuantity, opt => opt.MapFrom(ce => ce.CartItems.Sum(x => x.Quantity)))
            .ForMember(c => c.TotalPrice, opt => opt.MapFrom(ce => ce.CartItems.Sum(x => x.Quantity * x.UnitPrice)))
            .ForMember(c => c.Items, opt => opt.MapFrom(ce => ce.CartItems)); // tento řádek mi chyběl, ted už ok ?
    }
}
