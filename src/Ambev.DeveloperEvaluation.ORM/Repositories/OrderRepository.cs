using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DefaultContext _ctx;

    public OrderRepository(DefaultContext ctx) => _ctx = ctx;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _ctx.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> ListAsync(
        int pageNumber, int pageSize, string? search, string? sortBy, bool desc, CancellationToken ct = default)
    {
        var qry = _ctx.Orders.Include(o => o.Items).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();
            qry = qry.Where(o => o.Items.Any(i => i.Description.ToLower().Contains(term)));
        }

        // Ordenação simples por campo calculado/coluna
        qry = (sortBy?.ToLower()) switch
        {
            "createdat" => (desc ? qry.OrderByDescending(o => o.CreatedAt) : qry.OrderBy(o => o.CreatedAt)),
            "subtotal" => (desc ? qry.OrderByDescending(o => o.Subtotal) : qry.OrderBy(o => o.Subtotal)),
            "total" => (desc ? qry.OrderByDescending(o => o.Total) : qry.OrderBy(o => o.Total)),
            _ => (desc ? qry.OrderByDescending(o => o.CreatedAt) : qry.OrderBy(o => o.CreatedAt))
        };

        var count = await qry.CountAsync(ct);
        var items = await qry.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, count);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        _ctx.Orders.Add(order);
        await _ctx.SaveChangesAsync(ct);
        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        var db = await _ctx.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == order.Id, ct)
            ?? throw new KeyNotFoundException("Order not found");

        _ctx.Entry(db).CurrentValues.SetValues(order);

        foreach (var dbItem in db.Items.ToList())
        {
            if (!order.Items.Any(i => i.Id == dbItem.Id))
                _ctx.Remove(dbItem); // Deleted
        }

        foreach (var it in order.Items)
        {
            var existing = db.Items.FirstOrDefault(x => x.Id == it.Id);

            if (existing == null) // novo item
            {
                it.OrderId = db.Id;
                _ctx.Add(it); // Added
            }
            else // item existente -> atualiza valores
            {
                _ctx.Entry(existing).CurrentValues.SetValues(it); // Modified
            }
        }

        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _ctx.Orders.FirstOrDefaultAsync(o => o.Id == id, ct);
        if (entity != null)
        {
            _ctx.Orders.Remove(entity);
            await _ctx.SaveChangesAsync(ct);
        }
    }
}