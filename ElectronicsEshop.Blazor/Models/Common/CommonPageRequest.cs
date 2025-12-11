using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Models.Common;

public class CommonPageRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Role { get; set; }
    public OrderStatus? OrderStatus { get; set; }
}
