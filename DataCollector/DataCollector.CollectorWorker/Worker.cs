using DataCollector.Core;
using DataCollector.Data;

namespace DataCollector.CollectorWorker
{
    public class CollectorWorker : BackgroundService
    {
        private readonly ILogger<CollectorWorker> _logger;
        private readonly Collector _collector;
        private IConfiguration _configuration;
        private IMongoDbSettings _mongoDbSettings;

        public CollectorWorker(ILogger<CollectorWorker> logger, IConfiguration configuration, IMongoDbSettings mongoDbSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _configuration = configuration;
            _collector = new Collector();
            _mongoDbSettings = mongoDbSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mongoDbSettings.CreatorCollectionName = _configuration["MongoDbSetttings:CreatorCollectionName"];
            _mongoDbSettings.DatabaseName = _configuration["MongoDbSetttings:DatabaseName"];
            _mongoDbSettings.ConnectionString = _configuration["MongoDbSetttings:ConnectionString"];

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _collector.Run();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}