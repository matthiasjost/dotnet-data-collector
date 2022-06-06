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
                var htmlService = new HtmlService();

                htmlService.ParseRssAndAtomXmlLinks(httpService.ResponseString);

                Assert.True(htmlService.ExtractedRssXmlLinks.Count > 1, "Didn't find any URLs in HTML code!");
            }
            else
            {
                Assert.True(false, "Failed to load from Url!");
            }
        }
        [Fact]
        public async void TestIfYouTubeUserNameExtractFeeddUrls()
        {
            var httpService = new HttpService();

            httpService.Url = "https://youtube.com/user/IAmTimCorey";
            if (await httpService.TryUrlToString())
            {
                var htmlService = new HtmlService();
                htmlService.ParseRssAndAtomXmlLinks(httpService.ResponseString);
                Assert.True(htmlService.ExtractedRssXmlLinks.Count > 1, "Didn't find any URLs in HTML code!");
            }
            else
            {
                Assert.True(false, "Failed to load from Url!");
            }
        }
    }
}