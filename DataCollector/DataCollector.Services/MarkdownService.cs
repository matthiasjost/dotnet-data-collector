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
        public string InputMarkdownString { get; set; }
        public void ProcessMarkdownDocument(string markdown)
        {
            InputMarkdownString = markdown;
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            MarkdownDocument document = Markdown.Parse(markdown, pipeline);

            foreach (Block currentBlock in document)
            {
                if (currentBlock is Markdig.Extensions.Tables.Table)
                {
                    Markdig.Extensions.Tables.Table table = (Markdig.Extensions.Tables.Table)currentBlock;
                    ProcessTable(table);
                }
            }
        }
        public void ProcessTable(Markdig.Extensions.Tables.Table table)
        {
            foreach (Markdig.Extensions.Tables.TableRow tableRow in table)
            {
                ProcessTableRow(tableRow);
            }
        }
        public void ProcessTableRow(Markdig.Extensions.Tables.TableRow tableRow)
        {
            int columnIndex = 0;
            string name = "";
            string links = "";

            foreach (Markdig.Extensions.Tables.TableCell currentTableCell in tableRow)
            {
                foreach (Block currentBlock in currentTableCell)
                {
                    ProcessTableBlock(currentBlock);
                }
            }
        }

        public void ProcessTableBlock(Block block)
        {
            if (block is ParagraphBlock)
            {
                ParagraphBlock paragraphBlock = (ParagraphBlock)block;

                foreach (Markdig.Syntax.Inlines.Inline inlineElement in paragraphBlock.Inline)
                {
                    if (inlineElement is Markdig.Syntax.Inlines.LinkInline)
                    {
                        Markdig.Syntax.Inlines.LinkInline linkInLineELement = (Markdig.Syntax.Inlines.LinkInline)inlineElement;
                        ProcessLinkInLine(linkInLineELement);
                    }
                    else if (inlineElement is Markdig.Syntax.Inlines.Inline)
                    {
                        Markdig.Syntax.Inlines.Inline inline = (Markdig.Syntax.Inlines.Inline)inlineElement;
                        ProcessInLineElement(inline);

                    }
                }
            }
        }
        public void ProcessLinkInLine(Markdig.Syntax.Inlines.LinkInline linkInLineELement)
        {
            Console.WriteLine($"'{linkInLineELement.Url}'");
        }
        public void ProcessInLineElement(Markdig.Syntax.Inlines.Inline inLineElmement)
        {
            var text = InputMarkdownString.Substring(inLineElmement.Span.Start, inLineElmement.Span.End - inLineElmement.Span.Start + 1);
            Console.WriteLine($"'{text}'");
        }
    }
}
