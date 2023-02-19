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


        public string Type { get; set; }
        public string SectionTitle { get; set; }
        public string CountryCode { get; set; }
        public List<string> Tags { get; set; }
        public string Slogan { get; set; }
        public string Bio { get; set; }
    }
}