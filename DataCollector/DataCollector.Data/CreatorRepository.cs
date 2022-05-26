using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollector.Data
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly IMongoCollection<CreatorDbItem> _creatorCollection;

        public CreatorRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _creatorCollection = database.GetCollection<CreatorDbItem>(settings.CreatorCollectionName);
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
