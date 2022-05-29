using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Services.MarkdownDto
{
    public class RowDto
    {
        public int RowIndex { get; set; }
        public List<CellDto> Cells { get; set; }

        public RowDto(int rowIndex)
        {
            RowIndex = rowIndex;
        }
    }
}
