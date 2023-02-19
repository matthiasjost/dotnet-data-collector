using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class CreatorDto
    {
        public string Name { get; set; }
        public List<ChannelDto> Channels { get; set; }

        public CreatorType Type { get; set; }

        public string Section { get; set; }
        public string Country { get; set; }
        public List<string> Tags { get; set; }

        public CreatorDto()
        {
            Channels = new List<ChannelDto>();
        }
    }

    public enum CreatorType
    {
        CREATOR,
        OTHER
    }
}
