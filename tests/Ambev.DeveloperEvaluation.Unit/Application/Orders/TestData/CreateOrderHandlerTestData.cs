namespace Ambev.DeveloperEvaluation.Unit.Application.Orders.TestData;

using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;

public static class CreateOrderHandlerTestData
{
    public static CreateOrderCommand ValidOrderWithFiveUnits() =>
        new(new List<CreateOrderItemDto> {
            new("Pilsen 600ml", 10m, 5)
        });
}