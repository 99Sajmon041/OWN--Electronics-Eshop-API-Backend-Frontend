using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Products.Commands.CreateProduct;
using ElectronicsEshop.Application.Products.Commands.DeleteProduct;
using ElectronicsEshop.Application.Products.Commands.SetProductState;
using ElectronicsEshop.Application.Products.Commands.UpdateProduct;
using ElectronicsEshop.Application.Products.Commands.UpdateProductDiscount;
using ElectronicsEshop.Application.Products.Commands.UpdateProductsStockQty;
using ElectronicsEshop.Application.Products.DTOs;
using ElectronicsEshop.Application.Products.Queries.GetProduct;
using ElectronicsEshop.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProductListItemDto>>> GetAll([FromQuery] GetProductsQuery query, CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDetailDto>> Get([FromRoute] int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, [FromRoute] int id, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        await mediator.Send(new DeleteProductCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id}/discount")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDiscount([FromRoute] int id, [FromBody] UpdateProductDiscountCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id}/stock-qty")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStockQty([FromRoute] int id, [FromBody] UpdateProductsStockQtyCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id}/active")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProductsState([FromRoute] int id, [FromBody] SetProductStateCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }
}