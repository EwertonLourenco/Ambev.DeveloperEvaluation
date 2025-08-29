namespace Ambev.DeveloperEvaluation.Unit.Application.Orders;

using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Orders.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Order_And_Return_Totals()
    {
        var repo = Substitute.For<IOrderRepository>();
        repo.AddAsync(Arg.Any<Order>()).Returns(ci =>
        {
            var o = ci.Arg<Order>();
            o.Id = Guid.NewGuid();
            return o;
        });

        var handler = new CreateOrderHandler(repo);
        var cmd = CreateOrderHandlerTestData.ValidOrderWithFiveUnits();

        var result = await handler.Handle(cmd, default);

        await repo.Received(1).AddAsync(Arg.Any<Order>());
        result.Id.Should().NotBe(Guid.Empty);
        result.Subtotal.Should().Be(50m);
        result.DiscountPercent.Should().Be(0.10m);
        result.DiscountAmount.Should().Be(5m);
        result.Total.Should().Be(45m);
    }
}