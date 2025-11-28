using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Payments.DTOs;
using ElectronicsEshop.Application.Payments.Queries.GetPayments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<PaymentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<PaymentDto>>> GetAll([FromQuery] GetPaymentsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }
    }
}