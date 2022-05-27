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
        private readonly IMongoQueryable<CreatorDbItem> _queryableCreators;

        public CreatorRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _creatorCollection = database.GetCollection<CreatorDbItem>(settings.CreatorCollectionName);
            _queryableCreators = _creatorCollection.AsQueryable();
        }

        public async Task<CreatorDbItem> FindFirstWithName(string name)
        {
            var creatorDbItem = await _queryableCreators
                .Where<CreatorDbItem>(c => c.Name == name).FirstOrDefaultAsync();

            return creatorDbItem;
        }
        public List<CreatorDbItem> Get() =>
            _creatorCollection.Find(c => true).ToList();

        public CreatorDbItem Get(string id) =>
            _creatorCollection.Find<CreatorDbItem>(c => c.Id == id).FirstOrDefault();

        public CreatorDbItem Create(CreatorDbItem creator)
        {
            _creatorCollection.InsertOne(creator);
            return creator;
        }
        public void Update(string id, CreatorDbItem creator) =>
            _creatorCollection.ReplaceOne(c => c.Id == id, creator);

        public void Remove(CreatorDbItem creator) =>
            _creatorCollection.DeleteOne(c => c.Id == creator.Id);

        public void Remove(string id) =>
            _creatorCollection.DeleteOne(c => c.Id == id);
    }
}
