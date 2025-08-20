using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.ORM;

public sealed class DefaultContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                  ?? "Development";

        var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                      ?? Environment.GetEnvironmentVariable("DEV_PG_CONN");

        string conn;
        string source;

        if (!string.IsNullOrWhiteSpace(envConn))
        {
            conn = envConn!;
            source = "ENV";
        }
        else
        {
            var webApiDir = FindWebApiDir(AppContext.BaseDirectory);

            var cfg = new ConfigurationBuilder()
                .SetBasePath(webApiDir)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()  // <-- prioridade máxima
                .Build();

            conn = cfg.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não configurada.");
            source = $"appsettings ({env})";
        }

        var opts = new DbContextOptionsBuilder<DefaultContext>()
            .UseNpgsql(conn, b => b.MigrationsAssembly(typeof(DefaultContext).Assembly.GetName().Name))
            .Options;

        Console.WriteLine($"[EF-Design] ENV={env} | Source={source} | User={ExtractUser(conn)}");
        return new DefaultContext(opts);
    }

    private static string FindWebApiDir(string start)
    {
        var dir = new DirectoryInfo(start);
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Ambev.DeveloperEvaluation.WebApi",
                                         "Ambev.DeveloperEvaluation.WebApi.csproj");
            if (File.Exists(candidate))
                return Path.GetDirectoryName(candidate)!;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Não achei o projeto WebApi a partir de " + start);
    }

    private static string ExtractUser(string cs)
    {
        var parts = cs.Split(';', StringSplitOptions.RemoveEmptyEntries)
                      .Select(p => p.Split('=', 2))
                      .Where(a => a.Length == 2)
                      .ToDictionary(a => a[0].Trim().ToLowerInvariant(), a => a[1].Trim());
        return parts.TryGetValue("username", out var u) ? u :
               parts.TryGetValue("user id", out var u2) ? u2 : "?";
    }
}
