using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Common.Pagination;
using MediatR;

namespace ElectronicsEshop.Application.Carts.Queries.Admin.GetCarts;

public sealed class GetCartsQuery : IRequest<PagedResult<CartDetailDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? UserId { get; set; }
}
