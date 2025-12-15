using ElectronicsEshop.Application.Products.Shared.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommand : IRequest
{
    public required ProductUpsertDto Data { get; set; }
}
