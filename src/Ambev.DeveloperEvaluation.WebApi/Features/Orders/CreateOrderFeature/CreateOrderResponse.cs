namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;

public record CreateOrderResponse(
    Guid Id,
    decimal Subtotal,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total
);