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
        public async Task Run()
        {
            var httpService = new HttpService
            {
                Url = "https://raw.githubusercontent.com/matthiasjost/dotnet-content-creators/main/README.md"
            };

            if (await httpService.TryUrlToString() == false)
            {

            }




            var markdownService = new MarkdownTableService();
            markdownService.GenerateTableByMarkdownString(httpService.ResponseString);

            var creatorList = new CreatorListService(_creatorRepository);
            await creatorList.CheckBrokenLinks();



            creatorList.FillByTable(markdownService.TableList);
            creatorList.PrintCreators();
            await creatorList.AddCreatorsToDb();
            await creatorList.AddFeedUrlsFromHtml();

            await creatorList.PrintCreatorsFromDb();

            var youTubeService = new YouTubeApiService();
            youTubeService.GetVideo();
        }
    }
}
