using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public const int MAX_UNITS_PER_ITEM = 20;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new DomainException("Item description is required.");

        if (UnitPrice <= 0)
            throw new DomainException("Unit price must be greater than zero.");

        if (Quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (Quantity > MAX_UNITS_PER_ITEM)
            throw new DomainException($"Quantity cannot exceed {MAX_UNITS_PER_ITEM} units per item.");
    }

    public decimal LineTotal() => UnitPrice * Quantity;
}
