namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrderFeature;

public record UpdateOrderItemRequest(Guid? Id, string Description, decimal UnitPrice, int Quantity);

public class UpdateOrderRequest
{
    public Guid Id { get; set; }
    public IReadOnlyList<UpdateOrderItemRequest> Items { get; set; } = new List<UpdateOrderItemRequest>();
}