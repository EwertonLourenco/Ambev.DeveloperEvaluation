namespace Ambev.DeveloperEvaluation.Unit.Application.Orders;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Orders.ListOrders;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class ListOrdersHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Paged_Items_With_Correct_Metadata()
    {
        // arrange
        var repo = Substitute.For<IOrderRepository>();

        // cria 3 pedidos (vamos paginar 2 por página)
        var o1 = new Order();
        o1.AddItem(new OrderItem { Description = "IPA", UnitPrice = 10, Quantity = 5 });  // 10% desc

        var o2 = new Order();
        o2.AddItem(new OrderItem { Description = "Lager", UnitPrice = 12, Quantity = 2 });  // sem desc

        var o3 = new Order();
        o3.AddItem(new OrderItem { Description = "Weiss", UnitPrice = 15, Quantity = 10 }); // 20% desc

        var pageNumber = 1;
        var pageSize = 2;
        var search = (string?)null;
        var sortBy = "CreatedAt";
        var desc = true;

        repo.ListAsync(pageNumber, pageSize, search, sortBy, desc, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<(IReadOnlyList<Order>, int)>((new List<Order> { o3, o2 }, 3))); // simulando ordenação desc por data

        var handler = new ListOrdersHandler(repo);
        var query = new ListOrdersQuery(pageNumber, pageSize, search, sortBy, desc);

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // assert — metadados de paginação
        result.CurrentPage.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(2);
        result.HasPrevious.Should().BeFalse();
        result.HasNext.Should().BeTrue();

        // assert — itens mapeados
        result.Items.Should().HaveCount(2);
        result.Items[0].ItemsCount.Should().Be(1);
        result.Items[1].ItemsCount.Should().Be(1);

        // totais preservados do domínio
        // o3: 15 * 10 = 150, 20% = 30 => total 120
        result.Items[0].Total.Should().Be(120m);

        // o2: 12 * 2 = 24, sem desconto
        result.Items[1].Total.Should().Be(24m);
    }

    [Fact]
    public async Task Handle_Should_Call_Repository_With_Exact_Arguments()
    {
        // arrange
        var repo = Substitute.For<IOrderRepository>();

        repo.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<(IReadOnlyList<Order>, int)>((Array.Empty<Order>(), 0)));

        var handler = new ListOrdersHandler(repo);

        var query = new ListOrdersQuery(
            PageNumber: 3,
            PageSize: 25,
            Search: "ipa",
            SortBy: "Total",
            Desc: false
        );

        // act
        await handler.Handle(query, CancellationToken.None);

        // assert — garantimos que os mesmos parâmetros foram propagados ao repositório
        await repo.Received(1).ListAsync(
            pageNumber: 3,
            pageSize: 25,
            search: "ipa",
            sortBy: "Total",
            desc: false,
            Arg.Any<CancellationToken>()
        );
    }
}