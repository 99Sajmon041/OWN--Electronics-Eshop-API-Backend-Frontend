using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.CreateUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.DeleteUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.UpdateUserRole;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUser;
using ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUsers;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public class AdminUsersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ApplicationUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<ApplicationUserDto>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default, [FromQuery] string? role = null)
        {
            var result = await mediator.Send(new GetUsersQuery
            {
                Page = page,
                PageSize = pageSize,
                Role = role
            }, ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUserDto>> Get([FromRoute] string id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetUserQuery(id), ct);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
        {
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete([FromRoute]string id, CancellationToken ct)
        {
            await mediator.Send(new DeleteUserCommand(id), ct);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateUserRoleCommand command, CancellationToken ct)
        {
            command.Id = id;
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
