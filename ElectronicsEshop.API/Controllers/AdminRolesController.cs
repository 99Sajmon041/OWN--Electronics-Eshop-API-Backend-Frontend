using ElectronicsEshop.Application.Role.Queries.GetRole;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [Route("api/admin/users/roles")]
    [ApiController]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public class AdminRolesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetRoles()
        {
            var roles = Enum.GetValues<UserRoles>()
                .Select(role => role.ToString())
                .ToList();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> GetById([FromRoute] string id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRoleQuery(id), ct);
            return Ok(result);
        }
    }
}
