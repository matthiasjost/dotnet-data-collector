using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DataCollector.Services;
using DataCollector.Data;

namespace DataCollector.Core
{
    public class Collector : ICollector
    {
        ICreatorRepository _creatorRepository;

        public Collector(ICreatorRepository creatorRepository)
        {
            _creatorRepository = creatorRepository;
        }
        private CreatorList AllCreatorLinks { get; set; } = new CreatorList();
        public async Task Run()
        {
            var httpService = new HttpService();
            httpService.Open();
            httpService.Url = "https://raw.githubusercontent.com/matthiasjost/dotnet-content-creators/main/README.md";
            bool succesful = await httpService.TryUrlToString();
            var markstring = httpService.ResponseString;
            var markdownService = new MarkdownTableService();
            markdownService.GenerateTableByMarkdownString(markstring);
            FillCreatorListByMarkDownServiceTable(markdownService.TableList);


            PrintCreators();
            AddCreatorsToDb();

            var youTubeService = new YouTubeServiceHelper();
            youTubeService.GetVideo();

        }

        private async void AddCreatorsToDb()
        {
            foreach (Creator creator in AllCreatorLinks.List)
            {
                Console.Write(creator.Name);
                foreach (string url in creator.Urls)
                {
                    CreatorDbItem creatorFound = await _creatorRepository.FindFirstWithName(creator.Name);

                    if(creatorFound == null)
                    {
                        _creatorRepository.Create(new CreatorDbItem { Name = creator.Name });
                    }
   
                    Console.Write($" '{url}'");
                }
                Console.WriteLine();
            }
        }

        private void PrintCreators()
        {
            foreach (Creator creator in AllCreatorLinks.List)
            {
                Console.Write(creator.Name);
                foreach (string url in creator.Urls)
                {
                    Console.Write($" '{url}'");
                }
                Console.WriteLine();
            }
        }

        private void FillCreatorListByMarkDownServiceTable(List<TableDto> markDownServiceTable)
        {
            AllCreatorLinks.List = new List<Creator>();

            foreach (TableDto table in markDownServiceTable)
            {
                foreach (RowDto row in table.Rows)
                {
                    if (row.RowIndex != 0)
                    {
                        var creator = new Creator();

                        foreach (Cell cell in row.Cells)
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

                        AllCreatorLinks.List.Add(creator);
                    }
                }
            }
        }
    }
}
