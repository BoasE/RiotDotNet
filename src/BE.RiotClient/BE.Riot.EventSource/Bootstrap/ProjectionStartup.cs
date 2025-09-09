using System.Reflection;
using BE.CQRS.Di.AspCore;
using BE.CQRS.Domain;
using BE.CQRS.Domain.Configuration;
using BE.CQRS.Domain.Denormalization;
using BE.Learning.SharedKernel;
using BE.Riot.EventSource.Denormalizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BE.Riot.EventSource.Bootstrap
{
    public static class ProjectionStartup
    {
        public static IServiceCollection AddCqrsProjections(this IServiceCollection services, IConfiguration config)
        {
            var asm = typeof(MatchHistoryDenormalizer).Assembly;
            Assembly[] denormilizerAssemblies = [asm];
            Console.WriteLine("Attaching Denormalizers...");
            string readDburl = config["read:db:Host"];
            string readdb = config["read:db:name"];

            var client = new MongoClient(readDburl);
            IMongoDatabase readDb = client.GetDatabase(readdb);

            var ctx = new DenormalizerContext(client, readDb);
            services.AddSingleton<IDenormalizerContext>(ctx);
            services.AddSingleton<IProjectionRebuilder>(x => new ProjectionRebuilder(
                x.GetRequiredService<IDomainObjectRepository>(),
                x.GetRequiredService<IImmediateConventionDenormalizerPipeline>()));

            AddCqrsDenormalizer(services, denormilizerAssemblies);

            Console.WriteLine("Denormalizers attached!");
            return services;
        }

        private static void AddCqrsDenormalizer(IServiceCollection services, params Assembly[] denormilizerAssemblies)
        {
            DenormalizerConfiguration deconfig = new DenormalizerConfiguration()
                .SetDenormalizerAssemblies(denormilizerAssemblies);

            services
                .AddServiceProviderDenormalizerActivator()
                .AddImmediateDenormalization()
                .AddDenormalization(deconfig)
                .AddProjectionBuilder();
        }
    }
}