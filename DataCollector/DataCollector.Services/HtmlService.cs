using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace DataCollector.Services
{
    public class HtmlService
    {
        public List<string> ExtractedRssXmlLinks { get; set; }
        public List<string> ExtractedAtomXmlLinks { get; set; }
        public List<string> ExtractedFeedXmlLinks { get; set; }

        public HtmlService()
        {
            ExtractedRssXmlLinks = new List<string>();
            ExtractedAtomXmlLinks = new List<string>();
            ExtractedFeedXmlLinks = new List<string>();
        }

        public async Task LoadHtmlAndParseFeedUrls(string url)
        {
            var httpService = new HttpService();
            httpService.Url = url;

            if (await httpService.TryUrlToString())
            {
                await ParseRssAndAtomXmlLinks(httpService.ResponseString);
            }
        }
        public async Task ParseRssAndAtomXmlLinks(string htmlCode)
        {
            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);

            using var doc = await context.OpenAsync(req => req.Content(htmlCode));

            QueryLinkTags(doc);
            QueryAnchorTags(doc);
        }

        private void QueryLinkTags(IDocument doc)
        {
            var links = doc.QuerySelectorAll("link");

            foreach (var link in links)
            {
                var typeName = link.GetAttribute("type");

                if (typeName == "application/rss+xml")
                {
                    string hrefUrl = link.GetAttribute("href");
                    ExtractedRssXmlLinks.Add(hrefUrl);
                }
                else if (typeName == "application/atom+xml")
                {
                    string hrefUrl = link.GetAttribute("href");
                    ExtractedAtomXmlLinks.Add(hrefUrl);
                }
            }
        }

        private void QueryAnchorTags(IDocument doc)
        {
            var anchors = doc.QuerySelectorAll("a");

            foreach (var anchor in anchors)
            {
                var hrefUrl = anchor.GetAttribute("href");

                if (hrefUrl != null && hrefUrl.EndsWith("feed.xml"))
                {
                    ExtractedFeedXmlLinks.Add(hrefUrl);
                }
            }
        }
    }
}
