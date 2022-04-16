using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DataCollector.Services
{
    public class Collector
    {
        public Collector()
        {

        }
        public async Task Run()
        {
            var httpService = new HttpService();
            httpService.Open();
            httpService.Url = "https://raw.githubusercontent.com/matthiasjost/dotnet-content-creators/main/README.md";

            bool succesful = await httpService.TryUrlToString();
            var markstring = httpService.ResponseString;

            var markdownService = new MarkdownService();

            markdownService.ParseMarkdown(markstring);
        }
    }
}
