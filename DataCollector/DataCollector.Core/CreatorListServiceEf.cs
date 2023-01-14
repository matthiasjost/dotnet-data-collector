using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollector.Data;
using DataCollector.Services.MarkdownDto;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Core
{
    public class CreatorListServiceEf : ICreatorListService
    {
        private List<CreatorDto> ListOfCreatorDtos { get; set; }

        public CreatorListServiceEf()
        {
            ListOfCreatorDtos = new List<CreatorDto>();
        }

        public async Task AddCreatorsToDb()
        {
            using var db = new CreatorContext();

            foreach (CreatorDto creatorDto in ListOfCreatorDtos)
            {
                CreatorEntity? creatorFound =
                    await db.Creator.Where(c => c.Name == creatorDto.Name).FirstOrDefaultAsync();

                var channels = new List<ChannelEntity>();

                foreach (LinkDto link in creatorDto.Links)
                {
                    channels.Add(new ChannelEntity() { Url = link.Url, Label = link.Label });
                }

                var creator = new CreatorEntity { Name = creatorDto.Name, Channels = channels, CountryOrSection = creatorDto.CountryOrSection };

                if (creatorFound == null)
                {
                    await db.Creator.AddAsync(creator);
                }
                else
                {
                    creator.Id = creatorFound.Id;
                    creatorFound.CountryOrSection = creatorDto.CountryOrSection;
                    creatorFound.Channels.AddRange(creatorDto.Links);
                }
            }
            await db.SaveChangesAsync();
        }

        public Task AddFeedUrlsFromHtml()
        {
            throw new NotImplementedException();
        }

        public Task PrintCreatorsFromDb()
        {
            throw new NotImplementedException();
        }

        public void PrintCreators()
        {
            throw new NotImplementedException();
        }

        public List<CreatorDto> MapTableToCreators(List<TableDto> listOfTables)
        {
            throw new NotImplementedException();
        }

        public Task<List<CreatorDto>> FillDtoListByDatabase()
        {
            throw new NotImplementedException();
        }

        public Task<List<CreatorDto>> FillDtoListByDatabase(string searchValue)
        {
            throw new NotImplementedException();
        }

        public Task CheckBrokenLinks()
        {
            throw new NotImplementedException();
        }

        public Task PrintOpml()
        {
            throw new NotImplementedException();
        }

        public Task PrintOpmlYouTube()
        {
            throw new NotImplementedException();
        }
    }
}
