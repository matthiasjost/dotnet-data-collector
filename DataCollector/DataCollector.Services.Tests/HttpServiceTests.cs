namespace DataCollector.Services.Tests
{
    public class HttpServiceTests
    {
        [Fact]
        public async void TestIfContentReceived()
        {
            var httpService = new HttpService();

            httpService.Url = "https://www.matthias-jost.ch";
            if (await httpService.TryUrlToString())
            {


            }

            Assert.True(httpService.ResponseString.Length > 100);
        }
    }
}