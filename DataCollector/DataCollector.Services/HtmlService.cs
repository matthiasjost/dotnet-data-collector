using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AngleSharp;

namespace DataCollector.Services
{
    public class HtmlService
    {
        public List<string> ExtractedRssXmlLinks { get; set; } = new List<string>();
        public async void ParseRssXmlLinks(string htmlCode)
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
            }
        }
    }
}
