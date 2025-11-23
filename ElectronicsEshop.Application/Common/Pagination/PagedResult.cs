using ElectronicsEshop.Application.Common.Enums;

namespace ElectronicsEshop.Application.Common.Pagination;

public class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public string? Sort { get; init; }
    public SortOrder? Order { get; init; }

    //public int Totalpages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}
