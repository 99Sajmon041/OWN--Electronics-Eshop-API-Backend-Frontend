namespace ElectronicsEshop.API.Requests.Product;

public sealed class UpdateProductImageRequest
{
    public IFormFile ImageFile { get; set; } = default!;
}
