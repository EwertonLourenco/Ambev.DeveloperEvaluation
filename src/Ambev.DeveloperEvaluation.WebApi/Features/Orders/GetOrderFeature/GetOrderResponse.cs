namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrderFeature;

public record GetOrderItemResponse(Guid Id, string Description, decimal UnitPrice, int Quantity, decimal LineTotal);

public record GetOrderResponse(
    Guid Id,
    DateTime CreatedAt,
    decimal Subtotal,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total,
    IReadOnlyList<GetOrderItemResponse> Items
);