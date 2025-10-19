using System.Runtime.Serialization;
using BE.Learning.History.Mongo.Framework;
using MongoDB.Bson.Serialization.Attributes;

namespace BE.Riot.Mongo.SummonerMatchHistories;

[BsonIgnoreExtraElements]
public sealed class MongoSummonerMatch : MongoEntityBase
{
    public MongoSummonerMatchId Ids {
        get;
        set;
    }

    public bool HasDetails { get; set; } = false;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime StartedAtUtc { get; set; }
}