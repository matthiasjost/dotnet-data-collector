using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Services.MarkdownDto
{
    public class CellDto
    {
        public int ColumnIndex { get; set; }
        public List<string> TextLiterals { get; set; } = new List<string>();
        public List<ChannelDto> Links { get; set; }

        public CellDto(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public string GetPlainText()
        {
            string plainText = "";

            foreach (string textLiteral in TextLiterals)
            {
                plainText += textLiteral;
            }

            return plainText;
        }
    }
}
