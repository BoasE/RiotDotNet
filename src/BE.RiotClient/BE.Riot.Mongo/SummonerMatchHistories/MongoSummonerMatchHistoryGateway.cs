using BE.Learning.History.Mongo.Framework;
using BE.Riot.MatchHistories;
using MongoDB.Driver;

namespace BE.Riot.Mongo.SummonerMatchHistories;

public sealed class MongoSummonerMatchHistoryGateway(IDenormalizerContext ctx) :
    MongoSingleCollectionRepositoryBase<MongoSummonerMatch>(ctx.Db, "SummonerMatchHistory"),
    ISummonerMatchHistoryIdsGateway
{
    public Task Add(string puuid, string matchId, DateTimeOffset timestamp)
    {
        var id = new MongoSummonerMatchId
        {
            MatchId = matchId,
            PuuId = puuid,
        };

        var dto = new MongoSummonerMatch()
        {
            Ids = id,
            TimestampUtc = timestamp.UtcDateTime,
            StartedAtUtc = timestamp.UtcDateTime,
        };

        return InsertDto(dto, CancellationToken.None);
    }

    public async Task<DateTimeOffset?> LatestMatchDateTime(string puuId, CancellationToken cancellationToken)
    {
        var query = Builders<MongoSummonerMatch>.Filter.Eq(x => x.Ids.PuuId, puuId);
        var sort = Sort.Descending(x => x.StartedAtUtc);

        var highest = await Collection.Find(query).Sort(sort).FirstOrDefaultAsync(cancellationToken);

        return highest != null ? new DateTimeOffset(highest.StartedAtUtc, TimeSpan.Zero) : null;
    }

    public async Task<DateTimeOffset?> OldestMatchDateTime(string puuId, CancellationToken cancellationToken)
    {
        var query = Builders<MongoSummonerMatch>.Filter.Eq(x => x.Ids.PuuId, puuId);
        var sort = Sort.Ascending(x => x.StartedAtUtc);

        var highest = await Collection.Find(query).Sort(sort).FirstOrDefaultAsync(cancellationToken);

        return highest != null ? new DateTimeOffset(highest.StartedAtUtc, TimeSpan.Zero) : null;
    }
}