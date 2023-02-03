using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Services.MarkdownDto
{
    public class ChannelDto
    {
        public string Label { get; set; }
        public string Url { get; set; }

        public List<FeedDto> Feeds { get; set; }
    }

    public class FeedDto
    {
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
