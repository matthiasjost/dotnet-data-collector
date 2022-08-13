using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class CreatorDto
    {
        public string Name { get; set; }
        public List<LinkDto> Links { get; set; }

        public CreatorDto()
        {
            Links = new List<LinkDto>();
        }
    }
}
