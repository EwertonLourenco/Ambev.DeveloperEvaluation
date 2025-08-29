namespace Ambev.DeveloperEvaluation.Application.Orders.ListOrders;

public record ListOrdersItem(Guid Id, DateTime CreatedAt, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total, int ItemsCount);
public record ListOrdersResult(IReadOnlyList<ListOrdersItem> Items, int CurrentPage, int PageSize, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious);