using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AngleSharp.Dom;
using DataCollector.Data;
using DataCollector.Services;
using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core
{
    public class CreatorListService : ICreatorListService
    {
        private ICreatorRepository _creatorRepository;
        private List<CreatorDto> ListOfCreatorDtos { get; set; }

        public CreatorListService(ICreatorRepository creatorRepository)
        {
            _creatorRepository = creatorRepository;
            ListOfCreatorDtos = new List<CreatorDto>();
        }

        public async Task AddCreatorsToDb()
        {
            foreach (CreatorDto creatorDto in ListOfCreatorDtos)
            {
                CreatorEntity creatorFound = await _creatorRepository.FindFirstByName(creatorDto.Name);

                var channels = new List<ChannelEntity>();

                foreach (LinkDto link in creatorDto.Links)
                {
                    channels.Add(new ChannelEntity() { Url = link.Url, Label = link.Label });
                }

                var creator = new CreatorEntity { Name = creatorDto.Name, Channels = channels, CountryOrSection = creatorDto.CountryOrSection };

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

        public async Task AddFeedUrlsFromHtml()
        {
            var listOfCreatorEntities = await _creatorRepository.GetAllItems();
            foreach (var creatorEntity in listOfCreatorEntities)
            {
                await AddFeedForCreatorEntity(creatorEntity);
            }
        }

        private async Task AddFeedForCreatorEntity(CreatorEntity creatorEntity)
        {
            
            foreach (var channel in creatorEntity.Channels)
            {
                if (channel.Label == "Blog RSS")
                {
                    channel.Feeds.Add(new FeedEntity()
                    {
                        Type = HtmlService.FeedType.BlogRssTag.ToString(),
                        Url = channel.Url
                    });
                }
                if (channel.Label == "Blog")
                {
                    channel.Feeds = new List<FeedEntity>();

                    HtmlService hService = new HtmlService();
                    await hService.LoadHtmlAndParseFeedUrls(channel.Url);

                    if (hService.FeedUrls.Count > 0)
                    {
                        foreach (HtmlService.FeedUrl feedUrl in hService.FeedUrls)
                        {
                            channel.Feeds.Add(new FeedEntity()
                            {
                                Url = feedUrl.Url,
                                Type = feedUrl.Type.ToString()

                            });
                        }
                    }
                }
            }

            await _creatorRepository.UpdateById(creatorEntity);
            Console.Write(".");
        }

        public async Task PrintCreatorsFromDb()
        {
            var listOfCreatorEntities = await _creatorRepository.GetAllItems();

            foreach (var creatorEntity in listOfCreatorEntities)
            {
                Console.Write($"{creatorEntity.Id}, {creatorEntity.Name}");

                foreach (var channel in creatorEntity.Channels)
                {
                    Console.Write($" '{channel.Url}'");
                }
                Console.WriteLine();
            }
        }

        public void PrintCreators()
        {
            foreach (CreatorDto creator in ListOfCreatorDtos)
            {
                Console.Write(creator.Name);
                foreach (LinkDto link in creator.Links)
                {
                    Console.Write($" '{link.Label}' = '{link.Url}'");
                }
                Console.WriteLine();
            }
        }

        public async Task<List<CreatorDto>> FillDtoListByDatabase()
        {
            return await FillDtoListByDatabase(string.Empty);
        }

        public async Task<List<CreatorDto>> FillDtoListByDatabase(string searchValue)
        {
            List<CreatorEntity> listOfCreatorEntities = new List<CreatorEntity>();

            if (searchValue == string.Empty)
            {
                listOfCreatorEntities = await _creatorRepository.GetAllItems();
            }
            else
            {
                listOfCreatorEntities = await _creatorRepository.GetItems(searchValue);
            }

            foreach (CreatorEntity creatorEntity in listOfCreatorEntities)
            {
                CreatorDto creatorDtoItem = MapEntityToDto(creatorEntity);
                ListOfCreatorDtos.Add(creatorDtoItem);
            }
            return ListOfCreatorDtos;
        }

        private CreatorDto MapEntityToDto(CreatorEntity creatorEntity)
        {
            CreatorDto creatorDto = new CreatorDto()
            {
                Name = creatorEntity.Name,
                CountryOrSection = creatorEntity.CountryOrSection,
            };

            foreach (ChannelEntity channel in creatorEntity.Channels)
            {
                LinkDto linkDto = new LinkDto();
                linkDto.Label = channel.Label;
                linkDto.Url = channel.Url;
                creatorDto.Links.Add(linkDto);
            }
            return creatorDto;
        }

        public List<CreatorDto> MapTableToCreators(List<TableDto> listOfTables)
        {
            ListOfCreatorDtos = new List<CreatorDto>();

            foreach (TableDto table in listOfTables)
            {
                foreach (RowDto row in table.Rows)
                {
                    if (row.RowIndex != 0)
                    {
                        var creator = new CreatorDto();

                        creator.CountryOrSection = table.Title;

                        foreach (CellDto cell in row.Cells)
                        {
                            if (cell.ColumnIndex == 0)
                            {
                                creator.Name = cell.GetPlainText();
                            }
                            else if (cell.ColumnIndex == 1)
                            {
                                creator.Links = new List<LinkDto>();
                                foreach (LinkDto link in cell.Links)
                                {
                                    creator.Links.Add(link);
                                }
                            }
                        }
                        ListOfCreatorDtos.Add(creator);
                    }
                }
            }

            return ListOfCreatorDtos;
        }

        public async Task CheckBrokenLinks()
        {
            var listOfCreatorEntities = await _creatorRepository.GetAllItems();

            foreach (var creatorEntity in listOfCreatorEntities)
            {


                foreach (var channel in creatorEntity.Channels)
                {
                    var brokenLinkCheckService = new BrokenLinkCheckService();
                    var successFlag = await brokenLinkCheckService.PerformCheck(channel.Url);
                    if (successFlag == false)
                    {
                        Console.WriteLine($"Label: '{creatorEntity.Name}', successFlag = '{successFlag}', '{channel.Url}'");
                    }
                    else
                    {
                        Console.Write(".");
                    }

                }
                Console.WriteLine();
            }
        }

        public async Task PrintOpml()
        {
            XElement opml =
                new XElement("opml",
                    new XAttribute("version", "1.0"),
                    new XElement("head",
                        new XElement("title", "dotnet-content-creators")),
                    new XElement("body",
                        new XElement("outline",
                        new XAttribute("text", "dotnet-content-creators"),
                        new XAttribute("title", "dotnet-content-creators")
                )));


            XElement body = opml.Element("body");
            XElement firstOutline = body.Element("outline");

            if (firstOutline == null)
            {
                return; 
            }

            var listOfCreatorEntities = await _creatorRepository.GetAllItems();

            var listOfCreatorEntitiesOrdered = listOfCreatorEntities.OrderBy(s => s.Name);


            foreach (var creatorEntity in listOfCreatorEntitiesOrdered)
            {
                var filteredChannels = creatorEntity.Channels.Where(c => c.Label == "Blog" || c.Label == "Blog RSS");

                foreach (var channel in filteredChannels)
                {
                    if (channel.Feeds != null)
                    {

                        var filteredFeeds = channel.Feeds.Where(f =>
                            !f.Url.Contains("youtube.com")
                            && !f.Url.EndsWith("/comments/feed/"));

                        var feed = filteredFeeds.FirstOrDefault();

                        if (feed != null)
                        {
                            string feedType = "rss";

                            if (feed.Type == HtmlService.FeedType.LinkTagAtomType.ToString())
                            {
                                feedType = "atom";
                            }
                            else if (feed.Type == HtmlService.FeedType.LinkTagRssType.ToString())
                            {
                                feedType = "rss";
                            }
                            else
                            {
                                feedType = "rss";
                            }

                            firstOutline.Add(
                                new XElement("outline",
                                    new XAttribute("type", feedType),
                                    new XAttribute("title", $"{creatorEntity.Name}"),
                                    new XAttribute("xmlUrl", feed.Url)
                                ));

                        }
                    }
                }
            }
            opml.Save("opml.xml");
            string str = File.ReadAllText("opml.xml");
            Console.WriteLine(str);
        }
    }
}
