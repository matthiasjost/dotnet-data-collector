using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Data
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string CreatorCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
 
    }
}
