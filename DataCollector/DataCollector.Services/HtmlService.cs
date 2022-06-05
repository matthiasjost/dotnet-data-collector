using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace DataCollector.Services
{
    public class HtmlService
    {
        public List<string> ExtractedRssXmlLinks { get; set; }
        public List<string> ExtractedAtomXmlLinks { get; set; }

        public HtmlService()
        {
            ExtractedRssXmlLinks = new List<string>();
            ExtractedAtomXmlLinks = new List<string>();
        }

        public async Task LoadHtmlAndParseFeedUrls(string url)
        {
            var httpService = new HttpService();
            httpService.Url = url;
            if (await httpService.TryUrlToString())
            {
                await ParseRssXmlLinks(httpService.ResponseString);
            }
        }
        public async Task ParseRssXmlLinks(string htmlCode)
        {
            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);
            
            using var doc = await context.OpenAsync(req => req.Content(htmlCode));

            var links = doc.QuerySelectorAll("link");

            foreach (var link in links)
            {
               var typeName = link.GetAttribute("type");

               if (typeName == "application/rss+xml")
               {
                   var hrefUrl = link.GetAttribute("href");

                   ExtractedRssXmlLinks.Add(hrefUrl);
               }
               if (typeName == "application/atom+xml")
               {
                   var hrefUrl = link.GetAttribute("href");

                   ExtractedAtomXmlLinks.Add(hrefUrl);
               }
            }
        }
    }
}
