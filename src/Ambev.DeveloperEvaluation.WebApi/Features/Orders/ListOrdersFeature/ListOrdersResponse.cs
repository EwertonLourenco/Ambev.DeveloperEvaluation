namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.ListOrdersFeature;

public record ListOrdersItemResponse(
    Guid Id, DateTime CreatedAt, decimal Subtotal, decimal DiscountPercent, decimal DiscountAmount, decimal Total, int ItemsCount
);

public record ListOrdersResponse(
    IReadOnlyList<ListOrdersItemResponse> Items,
    int CurrentPage,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasNext,
    bool HasPrevious
);