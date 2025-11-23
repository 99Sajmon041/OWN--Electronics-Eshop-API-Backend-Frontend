using AutoMapper;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Orders.Mapping;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderListItemDto>()
            .ForMember(o => o.ItemsCount, opt => opt.MapFrom(ord => ord.OrderItems.Count));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(oi => oi.ProductName, opt => opt.MapFrom(o => o.Product.Name))
            .ForMember(oi => oi.ImageUrl, opt => opt.MapFrom(o => o.Product.ImageUrl));

        CreateMap<Order, OrderDetailDto>()
            .ForMember(odt => odt.ItemsCount, opt => opt.MapFrom(o => o.OrderItems.Count));

        CreateMap<Order, AdminOrderListItemDto>()
            .ForMember(o => o.CustomerEmail, opt => opt.MapFrom(o => o.ApplicationUser.Email))
            .ForMember(o => o.CustomerFullName, opt => opt.MapFrom(o => o.ApplicationUser.FullName))
            .ForMember(o => o.ItemsCount, opt => opt.MapFrom(o => o.OrderItems.Count));

        CreateMap<OrderItem, AdminOrderItemDto>()
            .ForMember(oi => oi.ProductName, opt => opt.MapFrom(o => o.Product.Name))
            .ForMember(oi => oi.ImageUrl, opt => opt.MapFrom(o => o.Product.ImageUrl));

        CreateMap<Order, AdminOrderDetailDto>()
            .ForMember(od => od.CustomerId, opt => opt.MapFrom(o => o.ApplicationUserId))
            .ForMember(od => od.CustomerEmail, opt => opt.MapFrom(o => o.ApplicationUser.Email))
            .ForMember(od => od.CustomerFullName, opt => opt.MapFrom(o => o.ApplicationUser.FullName))
            .ForMember(od => od.ItemsCount, opt => opt.MapFrom(o => o.OrderItems.Count));
    }
}
