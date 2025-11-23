using MediatR;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProductImage;

public sealed class UpdateProductImageCommand : IRequest<string>
{
    public int Id { get; init; }
    public string ImageUrl { get; set; } = default!;
}
