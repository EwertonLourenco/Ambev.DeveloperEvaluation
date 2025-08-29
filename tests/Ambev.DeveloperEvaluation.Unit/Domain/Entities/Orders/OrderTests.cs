namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.Orders;

using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class OrderTests
{
    [Fact]
    public void AddItem_Should_Throw_When_Quantity_Exceeds_20()
    {
        var order = new Order();
        var item = new OrderItem { Description = "X", UnitPrice = 10, Quantity = 21 };

        Action act = () => order.AddItem(item);
        act.Should().Throw<DomainException>().WithMessage("*20*");
    }

    [Theory]
    [InlineData(2, 0.00)]
    [InlineData(5, 0.10)]
    [InlineData(10, 0.20)]
    public void DiscountPercent_Should_Respect_Thresholds(int qty, double expectedPct)
    {
        var order = new Order();
        order.AddItem(new OrderItem { Description = "A", UnitPrice = 100, Quantity = qty });

        order.DiscountPercent.Should().Be((decimal)expectedPct);
    }

    [Fact]
    public void Totals_Should_Be_Calculated_Correctly()
    {
        var order = new Order();
        order.AddItem(new OrderItem { Description = "A", UnitPrice = 100, Quantity = 5 });

        order.Subtotal.Should().Be(500m);
        order.DiscountAmount.Should().Be(50m);
        order.Total.Should().Be(450m);
    }
}