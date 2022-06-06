using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataCollector.Services.Tests
{
    public class YouTubeApiServiceTests
    {

        [Fact]
        public async void TestIfChannel()
        {
            string secret = GetYouTubeApiKey();
        }

        public string GetYouTubeApiKey()
        {
            var appSettings = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            string secret = appSettings["YouTubeApiKey"];
            return secret;
        }
    }
}
