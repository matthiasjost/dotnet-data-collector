namespace DataCollector.Data
{
    public interface IMongoDbSettings
    {
        public string CreatorCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}