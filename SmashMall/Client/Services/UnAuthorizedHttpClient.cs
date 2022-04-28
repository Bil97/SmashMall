using System.Net.Http;

namespace SmashMall.Client.Services
{
    public class UnAuthorizedHttpClient
    {
        public HttpClient Client { get; set; }
        public UnAuthorizedHttpClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}
