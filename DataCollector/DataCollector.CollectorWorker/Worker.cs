using DataCollector.Core;


namespace DataCollector.CollectorWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Collector _collector;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            _collector = new Collector();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _collector.Run();

                await Task.Delay(1000, stoppingToken);
  
            }
        }
    }
}