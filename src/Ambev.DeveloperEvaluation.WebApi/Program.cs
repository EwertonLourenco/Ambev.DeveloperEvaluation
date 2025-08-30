using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ambev.DeveloperEvaluation.ORM;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.AddDefaultLogging();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("dev", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost",        // UI em Docker (porta 80)
                            "http://localhost:80",
                            "http://127.0.0.1",
                            "http://127.0.0.1:80",
                            "http://localhost:4200",   // ng serve
                            "http://127.0.0.1:4200",
                            "http://localhost:8080",
                            "http://127.0.0.1:8080"
                        )
                        .AllowAnyHeader()    // inclui Authorization, Content-Type etc
                        .AllowAnyMethod();   // GET/POST/PUT/DELETE/OPTIONS
                                             // .AllowCredentials(); // só se usar cookies/session (não precisa p/ Bearer)
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

            // Habilita Swagger no container (independe do ambiente)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ambev Developer Evaluation API v1");
                // Se quiser o Swagger na raiz em vez de /swagger:
                // c.RoutePrefix = string.Empty;
            });

            // Se manter o Swagger em /swagger, redirecione a raiz:
            app.MapGet("/", () => Results.Redirect("/swagger"))
               .ExcludeFromDescription();

            // Se você usa HTTPS Redirection, evite no container (não há cert por padrão)
            if (!app.Environment.IsEnvironment("Docker") && !app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("dev");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapHealthChecks("/health");
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
