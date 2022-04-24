using System;
using System.Collections.Generic;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Extensions;

namespace DataCollector.Services
{
    public class LinkDto
    {
        public string Url { get; set; }

    }
    public class CellDto
    {
        public List<string> TextLiterals = new List<string>();
        public List<LinkDto> Links = new List<LinkDto>();
    }
    public class RowDto
    {
        public List<CellDto> Cells { get; set; }
    }
    public class TableDto
    {
        public List<RowDto> Rows { get; set; }
    }

    public class MarkdownService
    {

        List<TableDto> TableList = new List<TableDto>();


        public int TableNumber { get; set; }

        public int TableRowNumber { get; set; }
        public int TableCellNumber { get; set; }

        public void NextRow()
        {
            TableRowNumber++;
            TableCellNumber = 0;
            Console.WriteLine($"TableRowNumber: {TableRowNumber}");

            TableList[TableNumber - 1].Rows.Add(new RowDto());
            TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells = new List<CellDto>();

        }
        public void NextTableCell()
        {
            TableCellNumber++;
            Console.WriteLine($"TableCellNumber: {TableCellNumber}");
            TableList[TableNumber - 1].Rows[TableRowNumber-1].Cells.Add(new CellDto());
        }
        public void NextTable()
        {
            TableList.Add(new TableDto());
            TableRowNumber = 0;
            TableCellNumber = 0;
            TableNumber++;
            TableList.Add(new TableDto());
            TableList[TableNumber - 1].Rows = new List<RowDto>();
            Console.WriteLine($"TableNumber: {TableNumber}");

        }

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
            NextTable();
            foreach (Markdig.Extensions.Tables.TableRow tableRow in table)
            {
                ProcessTableRow(tableRow);
            }
        }
        public void ProcessTableRow(Markdig.Extensions.Tables.TableRow tableRow)
        {
            NextRow();
            foreach (Markdig.Extensions.Tables.TableCell currentTableCell in tableRow)
            {
                ProcessTableCell(currentTableCell);
            }
        }
        public void ProcessTableCell(Markdig.Extensions.Tables.TableCell tableCell)
        {
            NextTableCell();
            foreach (Block currentBlock in tableCell)
            {
                ProcessTableBlock(currentBlock);
            }
        }
        public void ProcessTableBlock(Block block)
        {
            if (block is ParagraphBlock)
            {
                ProcessParagraphBlock((ParagraphBlock)block);
            }
        }
        public void ProcessParagraphBlock(ParagraphBlock paragraphBlock)
        {
            foreach (Markdig.Syntax.Inlines.Inline inlineElement in paragraphBlock.Inline)
            {
                ProcessInLineElement(inlineElement);
            }

        }
        public void ProcessInLineElement(Markdig.Syntax.Inlines.Inline inLineElement)
        {
            if (inLineElement is Markdig.Syntax.Inlines.LinkInline)
            {
                Markdig.Syntax.Inlines.LinkInline linkInLineELement = (Markdig.Syntax.Inlines.LinkInline)inLineElement;
                Console.WriteLine($"'{linkInLineELement.Url}'");
                if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links = new List<LinkDto>();
                }

                LinkDto linkDto = new LinkDto();
                linkDto.Url = linkInLineELement.Url;
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links.Add(linkDto);
            }
            else if (inLineElement is Markdig.Syntax.Inlines.LiteralInline)
            {
                Markdig.Syntax.Inlines.LiteralInline literalInLine = (Markdig.Syntax.Inlines.LiteralInline)inLineElement;

                string literalValue = literalInLine.Content.Text.Substring(literalInLine.Content.Start, literalInLine.Content.End - literalInLine.Content.Start + 1);
                Console.WriteLine($"'{literalValue}'");

                if(TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals = new List<string>();
                }
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber-1].TextLiterals.Add(literalValue);
            }
        }
    }
}
