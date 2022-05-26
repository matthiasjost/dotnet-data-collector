using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataCollector.Data
{
    public class CreatorRepository
    {
        private readonly IMongoCollection<CreatorDbItem> _creatorCollection;

        public ClientStatsService(IW3CLogStatsDatabase settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _creatorCollection = database.GetCollection<CreatorDbItem>(settings.ClientStatsCollectionName);
        }

        public List<CreatorDbItem> Get() =>
            _creatorCollection.Find(stat => true).ToList();

        public ClientStats Get(string id) =>
            _creatorCollection.Find<ClientStats>(stat => stat.Id == id).FirstOrDefault();

        public ClientStats Create(ClientStats stat)
        {
            _creatorCollection.InsertOne(stat);
            return stat;
        }
        public void Update(string id, ClientStats statIn) =>
            _creatorCollection.ReplaceOne(stat => stat.Id == id, statIn);

        public void Remove(ClientStats statIn) =>
            _creatorCollection.DeleteOne(stat => stat.Id == statIn.Id);

        public void Remove(string id) =>
            _creatorCollection.DeleteOne(stat => stat.Id == id);
    }
}
