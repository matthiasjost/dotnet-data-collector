using DataCollector.Services;
using DataCollector.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var collector = new Collector();
await collector.Run();


