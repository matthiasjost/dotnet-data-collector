using DataCollector.Services.MarkdownDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Services
{
    public interface IMarkdownTableService
    {
        public List<TableDto> GenerateTableByMarkdownString(string markdown);

    }
}
