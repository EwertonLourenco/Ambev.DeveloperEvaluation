using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.Functional;

public static class TestConfig
{
    public static string GetConnectionString(string name = "DefaultConnection")
    {
        var env = Environment.GetEnvironmentVariable("IT_POSTGRES_CS");
        if (!string.IsNullOrWhiteSpace(env))
            return env;

        var webApiPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..", "..", "..", "..",
            "src", "Ambev.DeveloperEvaluation.WebApi");

        var config = new ConfigurationBuilder()
            .SetBasePath(webApiPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = config.GetConnectionString(name);
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException($"ConnectionString '{name}' não encontrada. Defina IT_POSTGRES_CS ou inclua appsettings.Test.json.");
        return cs!;
    }
}