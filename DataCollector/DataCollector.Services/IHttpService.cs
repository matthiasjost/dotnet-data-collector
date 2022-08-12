using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Services
{
    public interface IHttpService
    {
        public string Url { get; set; }
        public string ResponseString { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        public Task<bool> TryUrlToString();
    }
}
