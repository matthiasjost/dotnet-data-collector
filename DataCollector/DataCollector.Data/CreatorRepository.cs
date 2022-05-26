using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;

namespace DataCollector.Data
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly IMongoCollection<CreatorDbItem> _creatorCollection;
        private IMongoQueryable<CreatorDbItem> _queryableMongoCreatorCollection;

        public CreatorRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _creatorCollection = database.GetCollection<CreatorDbItem>(settings.CreatorCollectionName);

            _queryableMongoCreatorCollection = _creatorCollection.AsQueryable();
        }

        public async Task<CreatorDbItem> FindFirstWithName(string name)
        {
            var creatorDbItem = await _queryableMongoCreatorCollection
                .Where<CreatorDbItem>(creator => creator.Name.StartsWith(name))
                .FirstOrDefaultAsync();

            return creatorDbItem;
        }
        public List<CreatorDbItem> Get() =>
            _creatorCollection.Find(stat => true).ToList();

        public CreatorDbItem Get(string id) =>
            _creatorCollection.Find<CreatorDbItem>(stat => stat.Id == id).FirstOrDefault();

        public CreatorDbItem Create(CreatorDbItem creator)
        {
            _creatorCollection.InsertOne(creator);
            return creator;
        }
        public void Update(string id, CreatorDbItem creator) =>
            _creatorCollection.ReplaceOne(creator => creator.Id == id, creator);

        public void Remove(CreatorDbItem creator) =>
            _creatorCollection.DeleteOne(creator => creator.Id == creator.Id);

        public void Remove(string id) =>
            _creatorCollection.DeleteOne(creator => creator.Id == id);
    }
}
