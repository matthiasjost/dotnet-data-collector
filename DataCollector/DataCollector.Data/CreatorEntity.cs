using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollector.Data
{
    public class CreatorEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Urls { get; set; }
        public List<string> RssFeedUrls { get; set; }
        public List<string> AtomFeedUrls { get; set; }
    }
}