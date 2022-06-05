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
        public string Rss { get; set; }
        public string Atom { get; set; }
    }
}
