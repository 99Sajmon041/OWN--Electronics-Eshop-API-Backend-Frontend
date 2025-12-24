using ElectronicsEshop.API.Interfaces;

namespace ElectronicsEshop.API.Services;

public sealed class ProductImageService(IWebHostEnvironment env, ILogger<ProductImageService> logger) : IProductImageService
{
    private const string ProductsFolder = "images/products";

    public async Task<string> SaveImageAsync(IFormFile image, CancellationToken ct)
    {
        if (image is null || image.Length == 0)
            throw new ArgumentException("Soubor s obrázkem je prázdný.", nameof(image));

        var uploadsRootFolder = Path.Combine(env.WebRootPath, ProductsFolder);
        Directory.CreateDirectory(uploadsRootFolder);

        var ext = Path.GetExtension(image.FileName);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadsRootFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream, ct);

        var relativePath = Path.Combine(ProductsFolder, fileName).Replace('\\', '/');

        return relativePath;
    }

    public Task DeleteImageAsync(string imageUrl, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return Task.CompletedTask;

        try
        {
            var fullPath = Path.Combine(env.WebRootPath, imageUrl.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Nepodařilo se smazat obrázek {ImageUrl}", imageUrl);
        }

        return Task.CompletedTask;
    }
}
