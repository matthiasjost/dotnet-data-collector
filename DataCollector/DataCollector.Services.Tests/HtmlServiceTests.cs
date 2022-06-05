namespace DataCollector.Services.Tests
{
    public class HtmlServiceTests
    {
        [Fact]
        public async void TestIfExtractUrls()
        {
            var httpService = new HttpService();

            httpService.Url = "https://www.matthias-jost.ch";
            if (await httpService.TryUrlToString())
            {

                
            }



      
        }
    }
}