using ElectronicsEshop.Application.Products.Shared.DTOs;

namespace ElectronicsEshop.API.Requests.Product;

public sealed class CreateProductRequest
{
    public ProductUpsertDto Data { get; set; } = default!;
    public IFormFile ImageFile { get; set; } = default!;
}
