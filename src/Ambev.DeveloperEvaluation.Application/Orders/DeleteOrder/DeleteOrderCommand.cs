using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder;

public record DeleteOrderCommand(Guid Id) : IRequest<Unit>;