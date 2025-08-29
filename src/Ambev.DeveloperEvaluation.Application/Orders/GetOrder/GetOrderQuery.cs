using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder;

public record GetOrderQuery(Guid Id) : IRequest<GetOrderResult>;