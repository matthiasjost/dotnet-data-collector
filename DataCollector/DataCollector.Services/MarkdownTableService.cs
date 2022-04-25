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

    public class MarkdownTableService
    {

        public List<TableDto> TableList = new List<TableDto>();
        private int TableNumber { get; set; }
        private int TableRowNumber { get; set; }
        private int TableCellNumber { get; set; }

        private void NextRow()
        {
            TableRowNumber++;
            TableCellNumber = 0;
            TableList[TableNumber - 1].Rows.Add(new RowDto());
            TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells = new List<CellDto>();

        }
        private void NextTableCell()
        {
            TableCellNumber++;
            TableList[TableNumber - 1].Rows[TableRowNumber-1].Cells.Add(new CellDto());
        }
        private void NextTable()
        {
            TableRowNumber = 0;
            TableCellNumber = 0;
            TableNumber++;
            TableList.Add(new TableDto());
            TableList[TableNumber - 1].Rows = new List<RowDto>();
            Console.WriteLine($"TableNumber: {TableNumber}");

        }

        public string InputMarkdownString { get; set; }
        public void GenerateTableByMarkdownString(string markdown)
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
        private void ProcessTable(Markdig.Extensions.Tables.Table table)
        {
            NextTable();
            foreach (Markdig.Extensions.Tables.TableRow tableRow in table)
            {
                ProcessTableRow(tableRow);
            }
        }
        private void ProcessTableRow(Markdig.Extensions.Tables.TableRow tableRow)
        {
            NextRow();
            foreach (Markdig.Extensions.Tables.TableCell currentTableCell in tableRow)
            {
                ProcessTableCell(currentTableCell);
            }
        }
        private void ProcessTableCell(Markdig.Extensions.Tables.TableCell tableCell)
        {
            NextTableCell();
            foreach (Block currentBlock in tableCell)
            {
                ProcessTableBlock(currentBlock);
            }
        }
        private void ProcessTableBlock(Block block)
        {
            if (block is ParagraphBlock)
            {
                ProcessParagraphBlock((ParagraphBlock)block);
            }
        }
        private void ProcessParagraphBlock(ParagraphBlock paragraphBlock)
        {
            foreach (Markdig.Syntax.Inlines.Inline inlineElement in paragraphBlock.Inline)
            {
                ProcessInLineElement(inlineElement);
            }

        }
        private void ProcessInLineElement(Markdig.Syntax.Inlines.Inline inLineElement)
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
