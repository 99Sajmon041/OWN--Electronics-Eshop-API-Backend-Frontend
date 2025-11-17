using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.CreateUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.DeleteUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.UpdateUserRole;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUser;
using ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUsers;
using ElectronicsEshop.Application.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUsersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ApplicationUserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ApplicationUserDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var result = await mediator.Send(new GetUsersQuery
            {
                Page = page,
                PageSize = pageSize,
            }, ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApplicationUserDto>> Get([FromRoute] string id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetUserQuery(id), ct);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
        {
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute]string id, CancellationToken ct)
        {
            await mediator.Send(new DeleteUserCommand(id), ct);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateUserRoleCommand command, CancellationToken ct)
        {
            command.Id = id;
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
