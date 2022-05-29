using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Data;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class CreatorList
    {
        private ICreatorRepository _creatorRepository;
        public List<Creator> List { get; set; }

        public CreatorList(ICreatorRepository creatorRepository)
        {
            _creatorRepository = creatorRepository;
        }

        public async void AddCreatorsToDb()
        {
            foreach (Creator creator in List)
            {
                Console.Write(creator.Name);
                foreach (string url in creator.Urls)
                {
                    CreatorDbItem creatorFound = await _creatorRepository.FindFirstByName(creator.Name);

                    if (creatorFound == null)
                    {
                        await _creatorRepository.Create(new CreatorDbItem { Name = creator.Name });
                    }
                    else
                    {
                        
                    }
                    Console.Write($" '{url}'");
                }
                Console.WriteLine();
            }
        }

        public void PrintCreators()
        {
            foreach (Creator creator in List)
            {
                Console.Write(creator.Name);
                foreach (string url in creator.Urls)
                {
                    Console.Write($" '{url}'");
                }
                Console.WriteLine();
            }
        }
        public void FillByTable(List<TableDto> listOfTables)
        {
            List = new List<Creator>();

            foreach (TableDto table in listOfTables)
            {
                foreach (RowDto row in table.Rows)
                {
                    if (row.RowIndex != 0)
                    {
                        var creator = new Creator();

                        foreach (CellDto cell in row.Cells)
                        {
                            if (cell.ColumnIndex == 0)
                            {
                                creator.Name = cell.GetPlainText();
                            }
                            else if (cell.ColumnIndex == 1)
                            {
                                creator.Urls = new List<string>();
                                foreach (LinkDto link in cell.Links)
                                {
                                    creator.Urls.Add(link.Url);
                                }
                            }
                        }
                        List.Add(creator);
                    }
                }
            }
        }
    }
}
