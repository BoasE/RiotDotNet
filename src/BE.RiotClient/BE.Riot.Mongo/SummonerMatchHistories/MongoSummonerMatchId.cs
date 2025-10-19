using BE.Learning.History.Mongo.Framework;
using MongoDB.Bson.Serialization.Attributes;

namespace BE.Riot.Mongo.SummonerMatchHistories;

public sealed class MongoSummonerMatchId : MongoEntityBase
{
    public string MatchId { get; set; }
    public string PuuId { get; set; }


    
    
}