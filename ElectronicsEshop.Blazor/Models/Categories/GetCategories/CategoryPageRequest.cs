namespace ElectronicsEshop.Blazor.Models.Categories.GetCategories
{
    public class CategoryPageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
