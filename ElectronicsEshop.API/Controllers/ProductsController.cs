using ElectronicsEshop.API.Interfaces;
using ElectronicsEshop.API.Requests.Product;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Products.Commands.CreateProduct;
using ElectronicsEshop.Application.Products.Commands.DeleteProduct;
using ElectronicsEshop.Application.Products.Commands.SetProductState;
using ElectronicsEshop.Application.Products.Commands.UpdateProduct;
using ElectronicsEshop.Application.Products.Commands.UpdateProductDiscount;
using ElectronicsEshop.Application.Products.Commands.UpdateProductImage;
using ElectronicsEshop.Application.Products.Commands.UpdateProductsStockQty;
using ElectronicsEshop.Application.Products.DTOs;
using ElectronicsEshop.Application.Products.Queries.GetProduct;
using ElectronicsEshop.Application.Products.Queries.GetProducts;
using ElectronicsEshop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator, IProductImageService imageService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ProductListItemDto>>> GetAll([FromQuery] GetProductsQuery query, CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDetailDto>> Get([FromRoute] int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductQuery(id), ct);
        return Ok(result);
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromForm] CreateProductRequest request, CancellationToken ct)
    {
        var imageUrl = await imageService.SaveImageAsync(request.ImageFile, ct);
        request.Data.ImageUrl = imageUrl;

        var command = new CreateProductCommand
        {
            Data = request.Data 
        };

        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var imageUrl = await mediator.Send(new DeleteProductCommand(id), ct);
        await imageService.DeleteImageAsync(imageUrl, ct);

        return NoContent();
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPatch("{id:int:min(1)}/discount")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateDiscount([FromRoute] int id, [FromBody] UpdateProductDiscountCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPatch("{id:int:min(1)}/stock-qty")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateStockQty([FromRoute] int id, [FromBody] UpdateProductsStockQtyCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPatch("{id:int:min(1)}/active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProductsState([FromRoute] int id, [FromBody] SetProductStateCommand command, CancellationToken ct)
    {
        command.Id = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    [Authorize(Policy = PolicyNames.CanManageProducts)]
    [HttpPatch("{id:int:min(1)}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromForm] UpdateProductImageRequest  request, CancellationToken ct)
    {
        var newImageurl = await imageService.SaveImageAsync(request.ImageFile, ct);

        var command = new UpdateProductImageCommand
        {
            Id = id,
            ImageUrl = newImageurl
        };

        var oldImageUrl = await mediator.Send(command, ct);

        if (!string.Equals(oldImageUrl, newImageurl, StringComparison.OrdinalIgnoreCase))
            await imageService.DeleteImageAsync(oldImageUrl, ct);

        return NoContent();
    }
}