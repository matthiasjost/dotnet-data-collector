using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataCollector.Services
{
    public class HttpService
    {
        private HttpClient _httpClient { get; set; }

        public string Url { get; set; }
        public string ResponseString { get; set; }

        HttpResponseMessage ResponseMessage { get; set; }

        public HttpService()
        {
        }

        public void Open()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> TryUrlToString()
        {
            try
            {
                ResponseMessage = await _httpClient.GetAsync(Url);
                ResponseString = await ResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
