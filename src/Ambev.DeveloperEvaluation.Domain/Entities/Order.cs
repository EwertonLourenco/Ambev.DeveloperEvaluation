using Ambev.DeveloperEvaluation.Domain.Common;
//using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Order : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new();

    public decimal Subtotal { get; private set; }
    public decimal DiscountPercent { get; private set; } // 0, 0.10m, 0.20m
    public decimal DiscountAmount { get; private set; }
    public decimal Total { get; private set; }

    // Regras pedidas no desafio (ajustáveis):
    public const int MIN_ITEMS_FOR_DISCOUNT = 3; // mínimo de unidades no pedido para aplicar qualquer desconto
    public const int THRESHOLD_10 = 5;           // >= 5 unidades ⇒ 10%
    public const int THRESHOLD_20 = 10;          // >= 10 unidades ⇒ 20%

    public void AddItem(OrderItem item)
    {
        item.Validate();
        // Limite de 20 unidades do MESMO item
        if (item.Quantity > OrderItem.MAX_UNITS_PER_ITEM)
            throw new DomainException($"Quantity cannot exceed {OrderItem.MAX_UNITS_PER_ITEM} units per item.");

        Items.Add(item);
        RecalculateTotals();
    }

    public void ReplaceItems(IEnumerable<OrderItem> items)
    {
        var list = items?.ToList() ?? new List<OrderItem>();
        foreach (var i in list) i.Validate();
        if (list.Any(i => i.Quantity > OrderItem.MAX_UNITS_PER_ITEM))
            throw new DomainException($"Quantity cannot exceed {OrderItem.MAX_UNITS_PER_ITEM} units per item.");

        Items = list;
        RecalculateTotals();
    }

    public int TotalQuantity() => Items.Sum(i => i.Quantity);

    public void RecalculateTotals()
    {
        Subtotal = Items.Sum(i => i.LineTotal());

        DiscountPercent = 0m;
        var qty = TotalQuantity();

        // Descontos só se aplicam se atingir o mínimo de unidades
        if (qty >= MIN_ITEMS_FOR_DISCOUNT)
        {
            if (qty >= THRESHOLD_20) DiscountPercent = 0.20m;
            else if (qty >= THRESHOLD_10) DiscountPercent = 0.10m;
        }

        DiscountAmount = Math.Round(Subtotal * DiscountPercent, 2, MidpointRounding.AwayFromZero);
        Total = Subtotal - DiscountAmount;
        if (Total < 0) Total = 0;
    }
}