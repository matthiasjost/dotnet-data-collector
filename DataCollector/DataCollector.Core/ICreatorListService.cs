using DataCollector.Services.MarkdownDto;

namespace DataCollector.Core;

public interface ICreatorListService
{
    List<CreatorDto> ListOfCreatorDtos { get; set; }
    Task AddCreatorsToDb();
    Task AddFeedUrlsFromHtml();
    Task PrintCreatorsFromDb();
    void PrintCreators();
    void FillDtoListByMarkdownTable(List<TableDto> listOfTables);
    public Task<List<CreatorDto>> FillDtoListByDatabase();
    Task CheckBrokenLinks();
}