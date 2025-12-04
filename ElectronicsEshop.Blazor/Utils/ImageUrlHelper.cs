namespace ElectronicsEshop.Blazor.UI.Utils;

public static class ImageUrlHelper
{
    public static string GetImageUrl(this HttpClient httpClient, string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return string.Empty;
        }

        var baseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/');

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return relativePath.TrimStart('/');
        }

        return $"{baseUrl}/{relativePath.TrimStart('/')}";
    }
}
