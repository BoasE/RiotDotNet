using BE.CQRS.Domain.Denormalization;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BE.Riot.Console;

public static class Rebuilder
{
    public static async Task ClearAndReprojectAll(this ServiceProvider serviceProvider)
    {
        var ctx = serviceProvider.GetRequiredService<IDenormalizerContext>();
        var collections = await ctx.Db.ListCollectionNamesAsync();
        var items = await collections.ToListAsync();
        foreach (var collection in items)
        {
            var etnry = ctx.Db.GetCollection<BsonDocument>(collection);
            await etnry.DeleteManyAsync(new BsonDocument());
        }

        var rebuilder = serviceProvider.GetRequiredService<IProjectionRebuilder>();
        await rebuilder.Execute(CancellationToken.None);
    }
}