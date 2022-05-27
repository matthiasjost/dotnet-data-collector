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
        public async Task<CreatorDbItem> FindFirstByName(string name)
        {
            var creatorDbItem = await _queryableCreators
                .Where<CreatorDbItem>(c => c.Name == name).FirstOrDefaultAsync();
            return creatorDbItem;
        }
        public async Task<CreatorDbItem> FindFirstById(string id)
        {
            var creatorDbItem = await _queryableCreators
                .Where<CreatorDbItem>(c => c.Id == id).FirstOrDefaultAsync();

            return creatorDbItem;
        }
        public async Task<List<CreatorDbItem>> GetAllItems()
        {
            return await _queryableCreators.ToListAsync();
        }
        public async void Create(CreatorDbItem creator)
        {
            await _creatorCollection.InsertOneAsync(creator);
        }
        public async void UpdateById(string id, CreatorDbItem creator)
        {
            await _creatorCollection.ReplaceOneAsync(c => c.Id == id, creator);
        }
        public async void RemoveById(string id)
        {
            await _creatorCollection.DeleteOneAsync(c => c.Id == id);
        }
    }
}
