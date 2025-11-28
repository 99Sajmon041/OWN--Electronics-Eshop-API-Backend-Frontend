using AutoMapper;
using ElectronicsEshop.Application.Payments.DTOs;
using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Payments;

public sealed class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Payment, PaymentDto>();
    }
}
