using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _repo;

    public CreateOrderHandler(IOrderRepository repo) => _repo = repo;

    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();
        foreach (var i in request.Items)
        {
            order.AddItem(new OrderItem
            {
                Description = i.Description.Trim(),
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            });
        }
        order.RecalculateTotals();

        order = await _repo.AddAsync(order, cancellationToken);

        return new CreateOrderResult(order.Id, order.Subtotal, order.DiscountPercent, order.DiscountAmount, order.Total);
    }
}