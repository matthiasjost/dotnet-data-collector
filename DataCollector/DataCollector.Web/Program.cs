using Azure.Identity;
using DataCollector.Core;
using DataCollector.Data;
using DataCollector.Services;

namespace DataCollector.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddAzureKeyVault(
                    new Uri("https://dotnetvault01.vault.azure.net/"),
                    new DefaultAzureCredential());
            }

            builder.Services.AddSingleton<IMongoDbSettings, MongoDbSettings>();
            builder.Services.AddTransient<ICreatorRepository, CreatorRepository>();
            builder.Services.AddTransient<ICollector, Collector>();

            builder.Services.AddTransient<ICreatorListService, CreatorListService>();
            builder.Services.AddTransient<IHttpService, HttpService>();
            builder.Services.AddTransient<IMarkdownTableService, MarkdownTableService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}