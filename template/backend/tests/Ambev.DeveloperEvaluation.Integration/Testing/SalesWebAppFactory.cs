using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.WebApi;

namespace Ambev.DeveloperEvaluation.Integration.Testing;

public class SalesWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // 1) Força chave de Infra para usar repositório InMemory (sem banco)
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var dict = new Dictionary<string, string?>
            {
                ["SalesRepository"] = "InMemory"
            };
            cfg.AddInMemoryCollection(dict!);
        });

        // 2) Se o template registra DbContext, substitui por InMemoryDatabase
        builder.ConfigureServices(services =>
        {
            // remove qualquer DbContextOptions<DefaultContext>
            var toRemove = services.Where(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>)).ToList();
            foreach (var d in toRemove) services.Remove(d);

            // adiciona EF InMemory só para evitar qualquer conexão real
            services.AddDbContext<DefaultContext>(opt => opt.UseInMemoryDatabase("WebApiTests"));
        });
    }
}
