using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using Ambev.DeveloperEvaluation.Application.Orders.ListOrders;
using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : BaseController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseWithData<CreateOrderResult>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderQuery(id), ct);
        return Ok(ApiResponseWithData<GetOrderResult>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null, [FromQuery] string? sortBy = "CreatedAt", [FromQuery] bool desc = true, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListOrdersQuery(pageNumber, pageSize, search, sortBy, desc), ct);
        return Ok(ApiResponseWithData<ListOrdersResult>.Ok(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateOrderCommand cmd, CancellationToken ct)
    {
        if (cmd.Id != id) return BadRequest(ApiResponse.Error("Route id and payload id must match."));
        var result = await _mediator.Send(cmd, ct);
        return Ok(ApiResponseWithData<UpdateOrderResult>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteOrderCommand(id), ct);
        return NoContent();
    }
}