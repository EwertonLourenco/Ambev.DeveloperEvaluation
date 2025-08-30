namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrderFeature;

public record UpdateOrderResponse(Guid Id, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total);