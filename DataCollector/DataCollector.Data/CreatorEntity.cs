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
        public List<ChannelEntity> Channels { get; set; } = new List<ChannelEntity>();
        public string CountryOrSection { get; set; }
        public List<Tag> Tags { get; set; }
    }
}