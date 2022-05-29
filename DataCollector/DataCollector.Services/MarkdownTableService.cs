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
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var document = Markdown.Parse(markdown, pipeline);

            foreach (var block in document)
            {
                if (block is Table table)
                {
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

            foreach (var block in tableCell)
            {
                if (block is ParagraphBlock paragraphBlock)
                {
                    ProcessParagraphBlock(paragraphBlock);
                }
            }
        }

        private void ProcessParagraphBlock(ParagraphBlock paragraphBlock)
        {
            foreach (Inline inlineElement in paragraphBlock.Inline)
            {
                ProcessInLineElement(inlineElement);
            }
        }

        private void ProcessInLineElement(Inline inlineElement)
        {
            if (inlineElement is LinkInline linkInlineElement)
            {
                if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links = new List<LinkDto>();
                }

                var linkDto = new LinkDto
                {
                    Url = linkInlineElement.Url
                };
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links.Add(linkDto);
            }
            else if (inlineElement is LiteralInline literalInline)
            {
                string literalValue = literalInline.Content.Text.Substring(literalInline.Content.Start, literalInline.Content.End - literalInline.Content.Start + 1);

                if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals == null)
                {
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals = new List<string>();
                }
                TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals.Add(literalValue);
            }
        }
    }
}
