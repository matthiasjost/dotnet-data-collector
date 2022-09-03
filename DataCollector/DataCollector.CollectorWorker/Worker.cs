using DataCollector.Core;
using DataCollector.Data;

namespace DataCollector.CollectorWorker
{
    public class CollectorWorker : BackgroundService
    {
        private readonly ILogger<CollectorWorker> _logger;
        private readonly ICollector _collector;
        private IConfiguration _configuration;
        private IMongoDbSettings _mongoDbSettings;

        public CollectorWorker(ILogger<CollectorWorker> logger, 
            IConfiguration configuration, 
            IMongoDbSettings mongoDbSettings, 
            ICollector collector)
        {
            _logger = logger;
            _configuration = configuration;
            _configuration = configuration;
            _collector = collector;
            _mongoDbSettings = mongoDbSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _collector.Run();

                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}