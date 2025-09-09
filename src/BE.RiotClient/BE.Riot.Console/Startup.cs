using System.Net.Http.Headers;
using BE.CQRS.Domain.Configuration;
using BE.Riot.EventSource.Bootstrap;
using BE.Riot.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace BE.Riot.Console;

public static class Startup
{
    public static IServiceCollection ConfigureServices()
    {
        // Konfiguration aufbauen (inkl. optionaler appsettings.{Environment}.json)
        var environment =
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

// DI-Container einrichten
        var services = new ServiceCollection();
        services.AddOptions();
        services.Configure<RiotAccountOptions>(configuration.GetSection("Riot:Account"));

        services.AddServices(configuration);
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
     
        });

        services.AddSingleton<IRiotClient>(sp =>
        {
            RiotAccountOptions opts = sp.GetRequiredService<IOptions<RiotAccountOptions>>().Value;
            if (string.IsNullOrWhiteSpace(opts.Host))
                throw new InvalidOperationException("Riot:Account:Host ist nicht konfiguriert.");

            if (string.IsNullOrWhiteSpace(opts.ApiKey))
                throw new InvalidOperationException("Riot:Account:ApiKey ist nicht konfiguriert.");

            // Hinweis: Passen Sie den Konstruktor an die tatsächliche RiotClient-API an.
            return new RiotHttpClient(opts.Host, opts.ApiKey);
        });

        services.AddCqrs(configuration);
        
        
        services.AddCqrsProjections(configuration);
        return services;
    }
}