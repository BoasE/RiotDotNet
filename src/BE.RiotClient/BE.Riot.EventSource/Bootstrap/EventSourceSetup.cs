using BE.CQRS.Data.MongoDb;
using BE.CQRS.Di.AspCore;
using BE.CQRS.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BE.Riot.EventSource.Bootstrap
{
    public static class EventSourceSetup
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services, IConfiguration config
        )
        {
            Console.WriteLine("Adding CQRS...");
            string eventSecret = "0mDJVERJ34e4qLC6JYvT!$_d#+54d";
            string url = config["events:db:host"];
            string dbName = config["events:db:name"];

            var client = new MongoClient(url);
            var db = client.GetDatabase(dbName);

            Console.WriteLine($"ES DB: {url} - {db}");

            var esconfig = new EventSourceConfiguration()
                .SetEventSecret(eventSecret)
                .SetDomainObjectAssemblies(typeof(PlayerMatchHistoryDomainObject).Assembly);

            services
                .AddServiceProviderDomainObjectAcitvator()
                .AddMongoDomainObjectRepository(db)
                .AddConventionBasedInMemoryCommandBus(esconfig)
                .AddEventSource(esconfig);

            services.AddSingleton<IEventSourceData>(x
                => new EventSourceData(client, db));

            Console.WriteLine("CQRS added");
            return services;
        }
    }
}