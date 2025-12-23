using ElectronicsEshop.Application.Contacts.SendContactMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/contact")]
    [AllowAnonymous]
    public class ContactsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Consumes("application/json")]
        [EnableRateLimiting("contact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendContactMessage([FromBody] SendContactMessageCommand command, CancellationToken ct)
        {
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
