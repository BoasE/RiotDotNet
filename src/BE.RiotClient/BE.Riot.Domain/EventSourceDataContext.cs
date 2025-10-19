using MongoDB.Driver;

namespace BE.Riot
{
    public interface IEventSourceData
    {
        IMongoDatabase Db { get; }

        IMongoClient Client { get; }
    }

    public sealed class EventSourceData : IEventSourceData
    {
        public EventSourceData(IMongoClient client, IMongoDatabase db)
        {
            Client = client;
            Db = db;
        }

        public IMongoDatabase Db { get; }

        public IMongoClient Client { get; }
    }
}