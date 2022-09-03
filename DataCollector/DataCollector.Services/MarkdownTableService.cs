using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCollector.Services.MarkdownDto;
using Markdig;
using Markdig.Syntax;
using Markdig.Extensions.Tables;
using Markdig.Syntax.Inlines;


namespace DataCollector.Services
{
    public class MarkdownTableService : IMarkdownTableService
    {
        private List<TableDto> TableList { get; set; } = new List<TableDto>();
        private int TableNumber { get; set; }
        private int TableRowNumber { get; set; }
        private int TableCellNumber { get; set; }

        private void NextTable(List<string> tableLiterals)
        {
            TableRowNumber = 0;
            TableCellNumber = 0;
            TableNumber++;
            TableList.Add(new TableDto() { Title = tableLiterals.Last() });
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

        public List<TableDto> GenerateTableByMarkdownString(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var document = Markdown.Parse(markdown, pipeline);
            List<string> tableStringLiterals = new List<string>();

            foreach (var block in document)
            {
                if (block is HeadingBlock headingBlock)
                {
                    ExtractLiteralsFromTableHeading(headingBlock, tableStringLiterals);
                }
                else if (block is Table table)
                {
                    ProcessTable(table, tableStringLiterals);
                    tableStringLiterals = new List<string>();
                }
            }

            return TableList;
        }

        private static void ExtractLiteralsFromTableHeading(HeadingBlock headingBlock, List<string> tableStringLiterals)
        {
            if (headingBlock.Inline is ContainerInline containerInline)
            {
                if (containerInline.FirstChild is LiteralInline literalInline)
                {
                    string tableLiteral = literalInline.Content.Text.Substring(literalInline.Content.Start,
                        literalInline.Content.End - literalInline.Content.Start + 1);

                    tableStringLiterals.Add(tableLiteral);
                }
            }
        }

        private void ProcessTable(Table table, List<string> tableLiterals)
        {
            NextTable(tableLiterals);

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
            switch (inlineElement)
            {
                case LinkInline linkInlineElement:
                {
                    if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links == null)
                    {
                        TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links = new List<LinkDto>();
                    }

                    var linkDto = new LinkDto
                    {
                        Url = linkInlineElement.Url
                    };

                    var firstChild = linkInlineElement.FirstChild;
                    if (firstChild != null && firstChild is LiteralInline literalInline )
                    {
                        linkDto.Label = literalInline.ToString();
                    }

                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].Links.Add(linkDto);
                    break;
                }
                case LiteralInline literalInline:
                {
                    string literalValue = literalInline.Content.Text.Substring(literalInline.Content.Start, literalInline.Content.End - literalInline.Content.Start + 1);

                    if (TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals == null)
                    {
                        TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals = new List<string>();
                    }
                    TableList[TableNumber - 1].Rows[TableRowNumber - 1].Cells[TableCellNumber - 1].TextLiterals.Add(literalValue);
                    break;
                }
            }
        }
    }

}
