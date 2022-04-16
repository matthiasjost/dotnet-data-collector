using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using Markdig.Extensions;

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

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            MarkdownDocument document = Markdown.Parse(httpService.ResponseString, pipeline);

            foreach (var d in document)
            {
                if (d is Markdig.Extensions.Tables.Table)
                {
                    var table = (Markdig.Extensions.Tables.Table)d;

                    foreach (Markdig.Extensions.Tables.TableRow tableRow in table)
                    {
                        foreach(Markdig.Extensions.Tables.TableCell tableCell in tableRow)
                        {
                            Console.WriteLine(markstring.Substring(tableCell.Span.Start, tableCell.Span.End - tableCell.Span.Start));
                        }
                    }
                }
            }
        }
    }
}
