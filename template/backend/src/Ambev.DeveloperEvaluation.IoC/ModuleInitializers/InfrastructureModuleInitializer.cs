using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // Sales repository: por enquanto só InMemory (com chave de comutação preparada)
        var kind = builder.Configuration.GetValue<string>("SalesRepository") ?? "InMemory";
        
        if (string.Equals(kind, "InMemory", StringComparison.OrdinalIgnoreCase))
        {
            builder.Services.AddSingleton<ISaleRepository, InMemorySaleRepository>();
        }
        // else: no futuro registramos uma implementação EF aqui
    }
}