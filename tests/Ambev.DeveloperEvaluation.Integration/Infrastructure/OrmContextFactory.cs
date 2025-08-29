using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

public static class OrmContextFactory
{
    public static DefaultContext Create(string cs)
    {
        var opts = new DbContextOptionsBuilder<DefaultContext>()
            .UseNpgsql(cs, b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"))
            .Options;
        var ctx = new DefaultContext(opts);
        ctx.Database.Migrate();
        return ctx;
    }
}