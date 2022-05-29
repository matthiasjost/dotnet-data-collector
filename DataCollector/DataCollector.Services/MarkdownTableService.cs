using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Services.MarkdownDto;
using Markdig;
using Markdig.Syntax;
using Markdig.Extensions.Tables;
using Markdig.Syntax.Inlines;


namespace DataCollector.Services
{
    public class MarkdownTableService
    {
        public string InputMarkdownString { get; set; }
        public List<TableDto> TableList = new List<TableDto>();
        private int TableNumber { get; set; }
        private int TableRowNumber { get; set; }
        private int TableCellNumber { get; set; }

        private void NextTable()
        {
            TableRowNumber = 0;
            TableCellNumber = 0;
            TableNumber++;
            TableList.Add(new TableDto());
            TableList[TableNumber - 1].Rows = new List<RowDto>();

        }
        private void NextRow()
        {
            TableRowNumber++;
            TableCellNumber = 0;
            TableList[TableNumber - 1].Rows.Add(new RowDto(TableRowNumber - 1));
            TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells = new List<CellDto>();

        }

        private void NextTableCell()
        {
            TableCellNumber++;
            TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells.Add(new CellDto(TableCellNumber - 1));
        }

        public void GenerateTableByMarkdownString(string markdown)
        {
            InputMarkdownString = markdown;
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            MarkdownDocument document = Markdown.Parse(markdown, pipeline);

            foreach (Block currentBlock in document)
            {
                if (currentBlock is Table)
                {
                    Table table = (Table)currentBlock;
                    ProcessTable(table);
                }
            }
        }
        private void ProcessTable(Table table)
        {
            NextTable();

            foreach (var block in table)
            {
                if (block is TableRow tableRow)
                {
                    ProcessTableRow(tableRow);
                }
            }
        }
        private void ProcessTableRow(TableRow tableRow)
        {
            NextRow();

            foreach (var block in tableRow)
            {
                if (block is TableCell tableCell)
                {
                    ProcessTableCell(tableCell);
                }
            }
        }
        private void ProcessTableCell(TableCell tableCell)
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
            foreach (Inline inlineElement in paragraphBlock.Inline)
            {
                ProcessInLineElement(inlineElement);
            }

        }
        private void ProcessInLineElement(Inline inLineElement)
        {
            if (inLineElement is LinkInline)
            {
                LinkInline linkInLineElement = (LinkInline)inLineElement;

                if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links = new List<LinkDto>();
                }

                LinkDto linkDto = new LinkDto();
                linkDto.Url = linkInLineElement.Url;
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links.Add(linkDto);
            }
            else if (inLineElement is LiteralInline)
            {
                LiteralInline literalInLine = (LiteralInline)inLineElement;

                string literalValue = literalInLine.Content.Text.Substring(literalInLine.Content.Start, literalInLine.Content.End - literalInLine.Content.Start + 1);


                if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals = new List<string>();
                }
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals.Add(literalValue);
            }
        }
    }
}
