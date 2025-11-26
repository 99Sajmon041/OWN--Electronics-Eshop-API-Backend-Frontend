using ElectronicsEshop.Application.Carts.Commands.AddCartItem;
using ElectronicsEshop.Application.Carts.Commands.DeleteCartItem;
using ElectronicsEshop.Application.Carts.Commands.DeleteCartItems;
using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Carts.Queries.Self.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(CartDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDetailDto>> Get(CancellationToken ct)
        {
            var result = await mediator.Send(new GetCartQuery(), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemCommand command, CancellationToken ct)
        {
            await mediator.Send(command, ct);
            return Ok();
        }

        [HttpDelete("items")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(CancellationToken ct)
        {
            await mediator.Send(new DeleteCartItemsCommand(), ct);
            return NoContent();
        }

        [HttpDelete("items/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await mediator.Send(new DeleteCartItemCommand(id), ct);
            return NoContent();
        }
    }
}
