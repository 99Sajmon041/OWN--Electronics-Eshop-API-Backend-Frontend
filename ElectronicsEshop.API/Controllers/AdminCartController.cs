using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Carts.Queries.Admin.GetCarts;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/admin/carts")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public class AdminCartController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CartDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<CartDetailDto>>> GetAll([FromQuery] GetCartsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }
    }
}
