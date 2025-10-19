using BE.Learning.History.Mongo.Framework;
using BE.Riot.MatchHistories;
using MongoDB.Driver;

namespace BE.Riot.Mongo.CompletedGameDetailsCache;

public sealed class MongoCompletedGameDetailsCache : MongoSingleCollectionRepositoryBase<MongoCompletedGameData>,
    ICompletedGameDetailCache
{
    public MongoCompletedGameDetailsCache(IEventSourceData ctx) : base(ctx.Db, "CompletedGameDetailsCache")
    {
    }
    public async Task<string?> GetCompletedGame(string gameId)
    {
        var query = Filters.Eq(x => x.RecordId, gameId);
        var entry = await Collection.Find(query).FirstOrDefaultAsync();
        return entry?.JsonData;
    }
    public Task SetCompletedGameData(string gameId, string content)
    {
        var dto = new MongoCompletedGameData()
        {
            
            RecordId = gameId,
            JsonData = content
        };

        return InsertDto(dto, CancellationToken.None);
    }
}