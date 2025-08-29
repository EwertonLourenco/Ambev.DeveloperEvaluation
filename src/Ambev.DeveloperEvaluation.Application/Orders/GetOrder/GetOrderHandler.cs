using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, GetOrderResult>
{
    private readonly IOrderRepository _repo;

    public GetOrderHandler(IOrderRepository repo) => _repo = repo;

    public async Task<GetOrderResult> Handle(GetOrderQuery request, CancellationToken ct)
    {
        var o = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Order not found");

        var items = o.Items.Select(i => new GetOrderItemDto(i.Id, i.Description, i.UnitPrice, i.Quantity, i.LineTotal())).ToList();
        return new GetOrderResult(o.Id, o.CreatedAt, o.Subtotal, o.DiscountPercent, o.DiscountAmount, o.Total, items);
    }
}