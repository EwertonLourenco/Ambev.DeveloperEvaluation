namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;

public record UpdateOrderResult(Guid Id, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total);