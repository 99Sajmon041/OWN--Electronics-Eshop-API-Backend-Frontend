using ElectronicsEshop.Application.Orders.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers;

[ApiController]
[Authorize]
[Route("api/users/")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}/orders/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> GetOrdersCoutForUser([FromRoute] string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetOrdersCountForUserQuery(id), ct);
        return Ok(result);
    }
}
