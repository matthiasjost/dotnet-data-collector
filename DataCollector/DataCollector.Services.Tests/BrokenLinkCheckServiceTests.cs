namespace DataCollector.Services.Tests
{
    public class BrokenLinkCheckServiceTests
    {
        [Fact]
        public async void TestIfPerformSuccessful1()
        {
            var service = new BrokenLinkCheckService();
            bool successFlag = await service.PerformCheck("https://www.matthias-jost.ch");
            Assert.True(successFlag);
        }
        [Fact]
        public async void TestIfPerformSuccessful2()
        {
            var service = new BrokenLinkCheckService();
            bool successFlag = await service.PerformCheck("https://blog.ndepend.com/");
            Assert.True(successFlag);
        }
        [Fact]
        public async void TestIfPerformSuccessful3()
        {
            var service = new BrokenLinkCheckService();
            bool successFlag = await service.PerformCheck("https://dotnetos.org/blog");
            Assert.True(successFlag);
        }
        [Fact]
        public async void TestIfPerformFails()
        {
            var service = new BrokenLinkCheckService();
            bool successFlag = await service.PerformCheck("https://www.matthias-jost2.ch");
            Assert.False(successFlag);
        }
    }
}