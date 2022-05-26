using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataCollector.Data
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public MongoDbSettings(IConfiguration configuration)
        {
            CreatorCollectionName = configuration["MongoDbSettings:CreatorCollectionName"];
            DatabaseName = configuration["MongoDbSettings:DatabaseName"];
            ConnectionString = configuration["MongoDbSettings:ConnectionString"];
        }
        public string CreatorCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
 
    }
}
