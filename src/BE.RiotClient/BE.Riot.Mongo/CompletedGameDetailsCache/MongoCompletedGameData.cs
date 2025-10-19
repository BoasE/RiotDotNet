using BE.Learning.History.Mongo.Framework;

namespace BE.Riot.Mongo.CompletedGameDetailsCache;

public sealed class MongoCompletedGameData : MongoEntityBase
{
    public string JsonData
    {
        get;
        set;
    }
}