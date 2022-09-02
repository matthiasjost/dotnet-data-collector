using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using static MongoDB.Driver.WriteConcern;

namespace DataCollector.Data
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly IMongoCollection<CreatorEntity> _creatorCollection;
        private readonly IMongoQueryable<CreatorEntity> _queryableCreators;

        public CreatorRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _creatorCollection = database.GetCollection<CreatorEntity>(settings.CreatorCollectionName);
            _queryableCreators = _creatorCollection.AsQueryable();
        }
        public async Task<CreatorEntity> FindFirstByName(string name)
        {
            var creatorDbItem = await _queryableCreators
                .Where<CreatorEntity>(c => c.Name == name).FirstOrDefaultAsync();
            return creatorDbItem;
        }
        public async Task<CreatorEntity> FindFirstById(string id)
        {
            var creatorDbItem = await _queryableCreators
                .Where<CreatorEntity>(c => c.Id == id).FirstOrDefaultAsync();

            return creatorDbItem;
        }
        public async Task<List<CreatorEntity>> GetAllItems()
        {
   
            return await _queryableCreators.ToListAsync();
        }

        public async Task<List<CreatorEntity>> GetItems(string searchValue)
        {
            var queryable = _queryableCreators.Where(c => c.Name.ToLower().Contains(searchValue.ToLower()));

            var executionModel = ((IMongoQueryable<CreatorEntity>)queryable).GetExecutionModel();
            var queryString = executionModel.ToString();

            return await _queryableCreators.Where(c => c.Name.ToLower().Contains(searchValue.ToLower())).ToListAsync();
        }

        public async Task Create(CreatorEntity creator)
        {
            await _creatorCollection.InsertOneAsync(creator);
        }
        public async Task UpdateById(CreatorEntity creator)
        {
            await _creatorCollection.ReplaceOneAsync(c => c.Id == creator.Id, creator);
        }
        public async Task RemoveById(string id)
        {
            await _creatorCollection.DeleteOneAsync(c => c.Id == id);
        }
    }
}
