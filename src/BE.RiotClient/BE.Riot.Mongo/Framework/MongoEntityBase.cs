
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BE.Learning.History.Mongo.Framework
{
    [BsonIgnoreExtraElements]
    public abstract class MongoEntityBase 
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string RecordId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedUtc { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime TimestampUtc { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime WriteTimestampUtc { get; set; }

        public virtual void Clear()
        {
            Id = ObjectId.Empty;
            RecordId = string.Empty;
            CreatedUtc = default;
            TimestampUtc = default;
            WriteTimestampUtc = default;
        }
    }
}