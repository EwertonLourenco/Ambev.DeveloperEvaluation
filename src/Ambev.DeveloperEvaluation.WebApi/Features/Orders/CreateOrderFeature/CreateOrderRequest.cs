namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;

public record CreateOrderItemRequest(string Description, decimal UnitPrice, int Quantity);

public class CreateOrderRequest
{
    public IReadOnlyList<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();
}