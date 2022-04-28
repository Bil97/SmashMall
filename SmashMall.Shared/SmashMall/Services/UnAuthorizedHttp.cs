using System;
using System.Net.Http;

namespace SmashMall.Services
{
    public class UnAuthorizedHttp
    {
        public UnAuthorizedHttp(Uri uri, HttpMessageHandler handler = null)
        {
            Client = new HttpClient(handler)
            {
                BaseAddress = uri
            };
        }

        public static HttpClient Client { get; set; }

    }
}

