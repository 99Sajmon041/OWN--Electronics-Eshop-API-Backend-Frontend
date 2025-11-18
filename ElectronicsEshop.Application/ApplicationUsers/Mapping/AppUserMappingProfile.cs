using AutoMapper;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.CreateUser;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Authorization.Commands.Register;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.ApplicationUsers.Mapping;

public class AppUserMappingProfile : Profile
{
    public AppUserMappingProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<ApplicationUser, ApplicationUserDto>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));

        CreateMap<CreateUserCommand, ApplicationUser>()
            .ForMember(x => x.Orders, opt => opt.Ignore())
            .ForMember(x => x.Cart, opt => opt.Ignore())
            .ForMember(x => x.Address, opt => opt.MapFrom(cuc => cuc.Address));

        CreateMap<RegisterAccountCommand, ApplicationUser>()
            .ForMember(x => x.Orders, opt => opt.Ignore())
            .ForMember(x => x.Cart, opt => opt.Ignore())
            .ForMember(x => x.Address, opt => opt.MapFrom(cuc => cuc.Address));
    }
}
