using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;

public record CreateOrderItemDto(string Description, decimal UnitPrice, int Quantity);
public record CreateOrderCommand(IReadOnlyList<CreateOrderItemDto> Items) : IRequest<CreateOrderResult>;