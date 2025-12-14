using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Common.Pagination;
using MediatR;

namespace ElectronicsEshop.Application.Carts.Queries.Admin.GetCarts;

public sealed class GetCartsQuery : IRequest<PagedResult<CartDetailDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; init; } = 5;
    public string? Email { get; set; }
}
