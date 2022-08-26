using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core;

public interface ICreatorListService
{ 
    Task AddCreatorsToDb();
    Task AddFeedUrlsFromHtml();
    Task PrintCreatorsFromDb();
    void PrintCreators();
    List<CreatorDto> MapTableToCreators(List<TableDto> listOfTables);
    public Task<List<CreatorDto>> FillDtoListByDatabase();
    Task CheckBrokenLinks();
}