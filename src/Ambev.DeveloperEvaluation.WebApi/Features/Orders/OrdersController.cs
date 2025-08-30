using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;

using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrderFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.ListOrdersFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrderFeature;

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
    private readonly IMapper _mapper;

    public OrdersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateOrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var validator = new CreateOrderRequestValidator();
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid) return BadRequest(validation.Errors);

        var cmd = _mapper.Map<CreateOrderCommand>(request);
        var result = await _mediator.Send(cmd, ct);
        var resp = _mapper.Map<CreateOrderResponse>(result);

        // use o CreatedAtAction do ControllerBase para não re-envelopar
        return base.CreatedAtAction(nameof(GetById), new { id = resp.Id },
            new ApiResponseWithData<CreateOrderResponse>
            {
                Success = true,
                Message = "Order created successfully",
                Data = resp
            });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderQuery(id), ct);
        var resp = _mapper.Map<GetOrderResponse>(result);

        // devolve o DTO puro; BaseController fará o envelope 1x
        return Ok(resp);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListOrdersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] ListOrdersRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<ListOrdersQuery>(request);
        var result = await _mediator.Send(query, ct);
        var resp = _mapper.Map<ListOrdersResponse>(result);

        return Ok(resp);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateOrderRequest request, CancellationToken ct)
    {
        // Se o corpo não enviou Id, use o da rota
        if (request.Id == Guid.Empty)
            request.Id = id;
        else if (request.Id != id)
            return BadRequest(ApiResponse.Error("Route id and payload id must match."));

        var validator = new UpdateOrderRequestValidator();
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid) return BadRequest(validation.Errors);

        var cmd = _mapper.Map<UpdateOrderCommand>(request);
        var result = await _mediator.Send(cmd, ct);
        var resp = _mapper.Map<UpdateOrderResponse>(result);

        return Ok(resp);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteOrderCommand(id), ct);
        return NoContent();
    }
}