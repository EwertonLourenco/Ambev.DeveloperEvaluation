using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResult>
{
    private readonly IOrderRepository _repo;

    public UpdateOrderHandler(IOrderRepository repo) => _repo = repo;

    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken ct)
    {
        var current = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Order not found");

        var items = request.Items.Select(i => new OrderItem
        {
            Id = i.Id ?? Guid.NewGuid(),
            OrderId = current.Id,
            Description = i.Description.Trim(),
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        });

        current.ReplaceItems(items);
        current.RecalculateTotals();
        await _repo.UpdateAsync(current, ct);

        return new UpdateOrderResult(current.Id, current.Subtotal, current.DiscountPercent, current.DiscountAmount, current.Total);
    }
}