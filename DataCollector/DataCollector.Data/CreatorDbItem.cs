using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollector.Data
{
    public class CreatorDbItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Urls { get; set; }
    }
}