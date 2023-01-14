using Azure.Identity;
using DataCollector.Core;
using DataCollector.Data;
using DataCollector.Services;
using Microsoft.Extensions.FileProviders;

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

            builder.Services.AddLogging();
            builder.Logging.AddConsole();

            /*builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
                new ManagedIdentityCredential());*/

            builder.Services.AddSingleton<IMongoDbSettings, MongoDbSettings>();
            builder.Services.AddTransient<ICreatorRepository, CreatorRepository>();
            builder.Services.AddTransient<ICollector, Collector>();

            builder.Services.AddTransient<ICreatorListService, CreatorListServiceEf>();
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

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "web\\dist"))
            });

            app.MapControllers();

            app.Run();
        }
    }
}