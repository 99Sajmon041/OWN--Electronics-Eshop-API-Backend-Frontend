using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Payments.DTOs;
using ElectronicsEshop.Application.Payments.Queries.GetPayments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.API.Controllers
{
    [Authorize(Policy = PolicyNames.AdminOnly)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPaymentsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult< PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<PaymentDto>>> GetAll([FromQuery] GetPaymentsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }
    }
}