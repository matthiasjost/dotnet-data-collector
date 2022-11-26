using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DataCollector.Services;
using DataCollector.Data;
using AngleSharp.Dom;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net.Http;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class Collector : ICollector
    {
        private ICreatorRepository _creatorRepository;
        private ICreatorListService _creatorListService;
        private IMarkdownTableService _markdownTableService;

        public Collector(ICreatorRepository creatorRepository, 
            ICreatorListService creatorListService,
            IMarkdownTableService markdownTableService)
        {
            _creatorRepository = creatorRepository;
            _creatorListService = creatorListService;
            _markdownTableService = markdownTableService;
        }
        public async Task Run()
        {
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync("https://raw.githubusercontent.com/matthiasjost/dotnet-content-creators/main/README.md");
            string markdownString = await httpResponseMessage.Content.ReadAsStringAsync();

            List<TableDto> tableList = _markdownTableService.GenerateTableByMarkdownString(markdownString);
            List<CreatorDto> creatorDtoList = _creatorListService.MapTableToCreators(tableList);
            
            await _creatorListService.AddCreatorsToDb();

            await _creatorListService.AddFeedUrlsFromHtml();

            _creatorListService.PrintCreators();

            await _creatorListService.PrintOpml();

            //await _creatorListService.CheckBrokenLinks();
        }
    }
}
