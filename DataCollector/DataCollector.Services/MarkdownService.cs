using System;
using System.Collections.Generic;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Extensions;

namespace DataCollector.Services
{
    public class MarkdownService
    {
        public void ParseMarkdown(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            MarkdownDocument document = Markdown.Parse(markdown, pipeline);

            foreach (var d in document)
            {
                if (d is Markdig.Extensions.Tables.Table)
                {
                    var table = (Markdig.Extensions.Tables.Table)d;

                    foreach (Markdig.Extensions.Tables.TableRow tableRow in table)
                    {
                        int columnIndex = 0;
                        string name = "";
                        string links = "";
                        foreach (Markdig.Extensions.Tables.TableCell tableCell in tableRow)
                        {
                            var columnString = markdown.Substring(tableCell.Span.Start, tableCell.Span.End - tableCell.Span.Start);
                            if (columnIndex == 0)
                            {
                                name = columnString;
                            }
                            else if (columnIndex == 1)
                            {
                                links = columnString;
                            }
                            columnIndex++;
                        }
                        Console.WriteLine($"Name: '{name}', Links: '{links}'");
                    }
                }
            }
        }
    }
}
