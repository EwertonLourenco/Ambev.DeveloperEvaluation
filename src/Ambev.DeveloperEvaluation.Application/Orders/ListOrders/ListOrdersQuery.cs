using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.ListOrders;

public record ListOrdersQuery(int PageNumber = 1, int PageSize = 10, string? Search = null, string? SortBy = "CreatedAt", bool Desc = true)
    : IRequest<ListOrdersResult>;