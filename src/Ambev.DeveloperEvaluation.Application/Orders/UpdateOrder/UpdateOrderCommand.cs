using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;

public record UpdateOrderItemDto(Guid? Id, string Description, decimal UnitPrice, int Quantity);
public record UpdateOrderCommand(Guid Id, IReadOnlyList<UpdateOrderItemDto> Items) : IRequest<UpdateOrderResult>;