using BE.CQRS.Domain.Events;
using MongoDB.Driver;

namespace BE.Learning.History.Mongo.Framework
{
    internal interface IMongoRepository
    {
        void CreateIndex();
    }

    public abstract class MongoSingleCollectionRepositoryBase<T> : IMongoRepository where T : MongoEntityBase, new()
    {
        // ReSharper disable once NotAccessedField.Global
 #pragma warning disable SA1401
        protected readonly string CollectionName;
        protected readonly IMongoCollection<T> Collection;
#pragma warning restore SA1401
        protected static readonly FilterDefinitionBuilder<T> Filters =
            Builders<T>.Filter;

        protected static readonly IndexKeysDefinitionBuilder<T> Keys =
            Builders<T>.IndexKeys;

        protected static readonly UpdateDefinitionBuilder<T> Updates =
            Builders<T>.Update;

        protected static readonly SortDefinitionBuilder<T> Sort =
            Builders<T>.Sort;

        private readonly TimeProvider _timeProvider;

        protected MongoSingleCollectionRepositoryBase(IMongoDatabase db, string collectionName, TimeProvider? provider = null)
        {
            Collection = db.GetCollection<T>(collectionName);
            CollectionName = collectionName;

            _timeProvider = provider ?? TimeProvider.System;
        }

        public virtual void CreateIndex()
        {
            CreateDefaultIndexes();
            CreateCustomIndexFields();
        }

        protected void CreateDefaultIndexes()
        {
            List<CreateIndexModel<T>> defaultIndex =
            [
                new (Keys.Ascending(x => x.RecordId)),
                new (Keys.Ascending(x => x.TimestampUtc)),
                new (Keys.Ascending(x => x.CreatedUtc)),
                new (Keys.Ascending(x => x.WriteTimestampUtc))
            ];

            Collection.Indexes.CreateMany(defaultIndex);
        }
        protected void CreateCustomIndexFields()
        {
            var customIndex = ProvideIndexList();

            if (customIndex != null && customIndex.Any())
            {
                Collection.Indexes.CreateMany(customIndex);
            }
        }

        private static readonly IList<CreateIndexModel<T>> Empty = new List<CreateIndexModel<T>>();
        protected virtual IList<CreateIndexModel<T>> ProvideIndexList()
        {
            return Empty;
        }

        private static readonly InsertOneOptions opt = new InsertOneOptions();

        internal Task InsertDto(T dto, CancellationToken cancellationToken)
        {
            dto.TimestampUtc = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(dto.RecordId))
            {
                dto.RecordId = Guid.CreateVersion7().ToString();
            }

            return Collection.InsertOneAsync(dto, opt, cancellationToken);
        }

        internal async Task InsertDto(Action<T> valueFactory, CancellationToken cancellationToken)
        {
            var dto = new T()
            {
                TimestampUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow,
                WriteTimestampUtc = _timeProvider.GetUtcNow().UtcDateTime,
            };

            valueFactory(dto);

            await Collection.InsertOneAsync(dto, opt, cancellationToken);
        }

        internal Task InsertEventDto(T dto, IEvent @event, CancellationToken cancellationToken)
        {
            dto.TimestampUtc = @event.Headers.Created.UtcDateTime;
            dto.CreatedUtc = dto.TimestampUtc;
            dto.WriteTimestampUtc = DateTime.UtcNow;

            return Collection.InsertOneAsync(dto, opt, cancellationToken);
        }

        internal async Task InsertEventDto(Action<T> valueFactory, IEvent @event, CancellationToken cancellationToken)
        {
            var dto = new T
            {
                TimestampUtc = @event.Headers.Created.UtcDateTime
            };

            dto.CreatedUtc = dto.TimestampUtc;
            dto.WriteTimestampUtc = DateTime.UtcNow;

            valueFactory(dto);

            await Collection.InsertOneAsync(dto, opt, cancellationToken);
        }

        internal Task UpdateOneDto(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken)
        {
            var time = DateTime.UtcNow;
            update = update.Set(x => x.TimestampUtc, time);

            return Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}