using ElectronicsEshop.Application.Account.Commands.ForgotPassword;
using ElectronicsEshop.Application.Account.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(ISender sender) : ControllerBase
    {
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return NoContent();
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
