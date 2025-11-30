using ElectronicsEshop.Application.Categories.Commands.CreateCategory;
using ElectronicsEshop.Application.Categories.Commands.DeleteCategory;
using ElectronicsEshop.Application.Categories.Commands.UpdateCategory;
using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Application.Categories.Queries.GetAllCategories;
using ElectronicsEshop.Application.Categories.Queries.GetCategories;
using ElectronicsEshop.Application.Categories.Queries.GetCategory;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<CategoryDto>>> GetAll([FromQuery] GetCategoriesQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }


        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetCategoryQuery(id), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
        {
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await mediator.Send(new DeleteCategoryCommand(id), ct);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryCommand command, CancellationToken ct)
        {
            command.Id = id;
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await mediator.Send(new GetAllCategoriesQuery(), ct);
            return Ok(result);
        }
    }
}
