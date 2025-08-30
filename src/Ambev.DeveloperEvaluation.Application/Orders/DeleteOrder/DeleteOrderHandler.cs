using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _repo;

    public DeleteOrderHandler(IOrderRepository repo) => _repo = repo;

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.Id, ct);
        return Unit.Value;
    }
}