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
                    channels.Add(new ChannelEntity() { Url = link.Url, Label = link.Label});
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
                var htmlSerivce = new HtmlService();
                foreach (var channel in creatorEntity.Channels)
                {
                    await htmlSerivce.LoadHtmlAndParseFeedUrls(channel.Url);
                    if (htmlSerivce.ExtractedRssXmlLinks.Count > 0)
                    {
                        foreach (var link in htmlSerivce.ExtractedRssXmlLinks)
                        {
                            if (channel.Feeds == null)
                            {
                                channel.Feeds = new List<FeedEntity>();
                            }
                            channel.Feeds.Add(new FeedEntity { Url = link, Type = "Rss"});
                        }
                    }
                    if (htmlSerivce.ExtractedAtomXmlLinks.Count > 0)
                    {
                        foreach (var link in htmlSerivce.ExtractedRssXmlLinks)
                        {
                            if (channel.Feeds == null)
                            {
                                channel.Feeds = new List<FeedEntity>();
                            }
                            channel.Feeds.Add(new FeedEntity { Url = link, Type = "Atom" });
                        }
                    }
                    if (htmlSerivce.ExtractedFeedXmlLinks.Count > 0)
                    {
                        foreach (var link in htmlSerivce.ExtractedFeedXmlLinks)
                        {
                            if (channel.Feeds == null)
                            {
                                channel.Feeds = new List<FeedEntity>();
                            }
                            channel.Feeds.Add(new FeedEntity { Url = link, Type = "Feed" });
                        }
                    }
                }
                await _creatorRepository.UpdateById(creatorEntity);
                Console.Write(".");
            }
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
                    new XAttribute("version", 2),
                    new XElement("head"),
                    new XElement("body")
                );


            XElement body = opml.Element("body");

            var listOfCreatorEntities = await _creatorRepository.GetAllItems();

            foreach (var creatorEntity in listOfCreatorEntities)
            {
                foreach (var channel in creatorEntity.Channels)
                {
                    if (channel.Feeds != null)
                    {
                        foreach (var feed in channel.Feeds)
                        {
                            body.Add(
                                new XElement("outline",
                                    new XAttribute("text", $"{creatorEntity.Name} - {channel.Label} - {feed.Type}"),
                                    new XAttribute("xmlUrl", feed.Url)));

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
