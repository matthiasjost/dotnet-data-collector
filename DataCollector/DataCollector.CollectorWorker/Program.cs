using DataCollector.CollectorWorker;
using DataCollector.Data;
using Microsoft.Extensions.Configuration;

namespace Company.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<CollectorWorker>();
                    services.AddSingleton<IMongoDbSettings, MongoDbSettings>();
                    services.AddSingleton<ICreatorRepository, CreatorRepository>();

                })
                .Build();



            host.Run();
        }
    }
}