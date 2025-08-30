using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.ListOrders;

public class ListOrdersHandler : IRequestHandler<ListOrdersQuery, ListOrdersResult>
{
    private readonly IOrderRepository _repo;
    public ListOrdersHandler(IOrderRepository repo) => _repo = repo;

    public async Task<ListOrdersResult> Handle(ListOrdersQuery request, CancellationToken ct)
    {
        var (items, total) = await _repo.ListAsync(request.PageNumber, request.PageSize, request.Search, request.SortBy, request.Desc, ct);
        var resultItems = items.Select(o => new ListOrdersItem(o.Id, o.CreatedAt, o.Subtotal, o.DiscountPercent, o.DiscountAmount, o.Total, o.Items.Count)).ToList();

        var totalPages = (int)Math.Ceiling(total / (double)request.PageSize);
        return new ListOrdersResult(resultItems, request.PageNumber, request.PageSize, total, totalPages, request.PageNumber < totalPages, request.PageNumber > 1);
    }
}