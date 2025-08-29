using System.Linq;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebAppFactory : WebApplicationFactory<Ambev.DeveloperEvaluation.WebApi.Program>
{
    private readonly string _cs;
    public CustomWebAppFactory(string connectionString) => _cs = connectionString;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var existing = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (existing is not null) services.Remove(existing);

            services.AddDbContext<DefaultContext>(opt =>
                opt.UseNpgsql(_cs, b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")));

            using var scope = services.BuildServiceProvider().CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            ctx.Database.Migrate();
        });
    }
}