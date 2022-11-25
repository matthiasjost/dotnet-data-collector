using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using static DataCollector.Services.HtmlService;

namespace DataCollector.Services
{
    public class HtmlService
    {
        public enum FeedType
        {
            LinkTagRssType = 1,
            LinkTagAtomType = 2,
            AHrefFeedType = 3
            
        }
        public class FeedUrl
        {

            public string Url { get; set; }
            public FeedType Type { get; set; }
        }

        public List<FeedUrl> FeedUrls { get; set; }

        public HtmlService()
        {
            FeedUrls = new List<FeedUrl>();
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
            QueryAHrefTags(doc);
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

                    FeedUrls.Add(new FeedUrl()
                    {
                        Type = FeedType.LinkTagRssType,
                        Url = hrefUrl

                    });
                }
                else if (typeName == "application/atom+xml")
                {
                    string hrefUrl = link.GetAttribute("href");

                    FeedUrls.Add(new FeedUrl()
                    {
                        Type = FeedType.LinkTagAtomType,
                        Url = hrefUrl

                    });
                }
            }
        }

        private void QueryAHrefTags(IDocument doc)
        {
            var anchors = doc.QuerySelectorAll("a");

            foreach (var anchor in anchors)
            {
                var hrefUrl = anchor.GetAttribute("href");

                if (hrefUrl != null && hrefUrl.EndsWith("feed.xml"))
                {
                    FeedUrls.Add(new FeedUrl()
                    {
                        Type = FeedType.AHrefFeedType,
                        Url = hrefUrl

                    });
                }
            }
        }
    }
}
