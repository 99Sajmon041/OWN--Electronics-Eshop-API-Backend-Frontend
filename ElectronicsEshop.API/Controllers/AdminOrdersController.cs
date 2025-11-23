using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Orders.Commands.Admin.UpdateOrderStatus;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Application.Orders.Queries.Admin.GetOrder;
using ElectronicsEshop.Application.Orders.Queries.Admin.GetOrders;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [Route("api/admin/orders")]
    [ApiController]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public class AdminOrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<AdminOrderListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<AdminOrderListItemDto>>> GetAll([FromQuery] GetOrdersQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(typeof(AdminOrderDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminOrderDetailDto>> Get([FromRoute] int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetOrderQuery(id), ct);
            return Ok(result);
        }


        [HttpPatch("{id:int:min(1)}/changestatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusCommand command, [FromRoute] int id, CancellationToken ct)
        {
            command.Id = id;
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
