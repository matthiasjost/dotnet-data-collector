using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollector.Data
{
    public class ChannelEntity
    {
        public string Url { get; set; }
        public string Label { get; set; }
        public List<FeedEntity> Feeds { get; set; } = new List<FeedEntity>();
    }
}
