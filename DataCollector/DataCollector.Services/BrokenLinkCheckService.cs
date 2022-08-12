using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Services
{
    public class BrokenLinkCheckService
    {
        public TimeSpan ResponseTime { get; set; }

        public async Task<bool> PerformCheck(string url)
        {
            var httpClient = new HttpClient();
            var stopWatch = Stopwatch.StartNew();
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

                ResponseTime = stopWatch.Elapsed;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                ResponseTime = stopWatch.Elapsed;

            }
            return true;
        }
    }
}
