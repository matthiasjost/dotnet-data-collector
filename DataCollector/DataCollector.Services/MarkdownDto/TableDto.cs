using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Services.MarkdownDto
{
    public class TableDto
    {
        public string Title { get; set; }
        public string CountrySvgHtmlCode { get; set; }
        public List<RowDto> Rows { get; set; }
    }
}
