using ElectronicsEshop.Application.ApplicationUsers.Commands.Self.DeleteUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUser;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUserPassword;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.ApplicationUsers.Queries.Self.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    [Authorize]
    public class MeController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApplicationUserDto>> Profile(CancellationToken ct)
        {
            var user = await sender.Send(new GetProfileQuery(), ct);
            return Ok(user);
        }

        [HttpPatch("update")]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApplicationUserDto>> Update([FromBody] UpdateProfileCommand command, CancellationToken ct)
        {
            var updatedUser = await sender.Send(command, ct);
            return Ok(updatedUser);
        }

        [HttpPatch("update-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(CancellationToken ct)
        {
            await sender.Send(new DeleteUserCommand(), ct);
            return NoContent();
        }
    }
}
