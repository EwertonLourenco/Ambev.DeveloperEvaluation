using System;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;

public record CreateOrderResult(Guid Id, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total);