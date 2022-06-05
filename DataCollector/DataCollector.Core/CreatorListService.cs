using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Data;
using DataCollector.Services;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class CreatorListService
    {
        private ICreatorRepository _creatorRepository;
        public List<CreatorDto> ListOfCreatorDtos { get; set; }

        public CreatorListService(ICreatorRepository creatorRepository)
        {
            _creatorRepository = creatorRepository;
        }

        public async Task AddCreatorsToDb()
        {
            foreach (CreatorDto creatorDto in ListOfCreatorDtos)
            {
                CreatorEntity creatorFound = await _creatorRepository.FindFirstByName(creatorDto.Name);

                var creator = new CreatorEntity { Name = creatorDto.Name, Urls = creatorDto.Urls };

                if (creatorFound == null)
                {
                    await _creatorRepository.Create(creator);
                }
                else
                {
                    creator.Id = creatorFound.Id;
                    await _creatorRepository.UpdateById(creator);
                }
            }
        }

        public async Task AddRssUrlsFromHtml()
        {
            var listOfCreatorEntities = await _creatorRepository.GetAllItems();
            foreach (var creatorEntity in listOfCreatorEntities)
            {
                var htmlSerivce = new HtmlService();
                foreach (string url in creatorEntity.Urls)
                {
                    await htmlSerivce.LoadHtmlAndParseFeedUrls(url); 

                }
                creatorEntity.RssFeedUrls = htmlSerivce.ExtractedRssXmlLinks;
                _creatorRepository.UpdateById(creatorEntity);

            }
        }

        public async Task PrintCreatorsFromDb()
        {
            var listOfCreatorEntities = await _creatorRepository.GetAllItems();

            foreach (var creatorEntity in listOfCreatorEntities)
            {
                Console.Write($"{creatorEntity.Id}, {creatorEntity.Name}");

                foreach (string url in creatorEntity.Urls)
                {
                    Console.Write($" '{url}'");
                }
                Console.WriteLine();
            }
        }
        public void PrintCreators()
        {
            foreach (CreatorDto creator in ListOfCreatorDtos)
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
            ListOfCreatorDtos = new List<CreatorDto>();

            foreach (TableDto table in listOfTables)
            {
                foreach (RowDto row in table.Rows)
                {
                    if (row.RowIndex != 0)
                    {
                        var creator = new CreatorDto();

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
                        ListOfCreatorDtos.Add(creator);
                    }
                }
            }
        }
    }
}
