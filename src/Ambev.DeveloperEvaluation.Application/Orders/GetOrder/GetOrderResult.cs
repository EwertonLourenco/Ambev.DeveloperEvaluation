namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder;

public record GetOrderItemDto(Guid Id, string Description, decimal UnitPrice, int Quantity, decimal LineTotal);
public record GetOrderResult(Guid Id, DateTime CreatedAt, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total, IReadOnlyList<GetOrderItemDto> Items);