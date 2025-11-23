namespace ElectronicsEshop.API.Interfaces
{
    public interface IProductImageService
    {
        Task<string> SaveImageAsync(IFormFile image, CancellationToken ct);
        Task DeleteImageAsync(string imageUrl, CancellationToken ct);
    }
}
