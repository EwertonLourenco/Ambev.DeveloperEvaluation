namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Ambev.DeveloperEvaluation.Domain.Entities;

public static class OrderTestData
{
    public static Order NewOrderWithItem(string desc, decimal unit, int qty)
    {
        var o = new Order();
        o.AddItem(new OrderItem { Description = desc, UnitPrice = unit, Quantity = qty });
        return o;
    }
}