using System;
using System.Net.Http;

namespace SmashMall.Services
{
    public class BaseApi
    {
        public BaseApi()
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            // new AuthorizedHttp(new Uri(Url), httpClientHandler);
            // new UnAuthorizedHttp(new Uri(Url), httpClientHandler);


        }
        public static string Url => "https://localhost:5001";

        public static string DefaultImageApi => $"{Url}/api/files/default/image";

        public static string ItemsPhotosApi => $"{Url}/api/files/item";

        public static string GoodsDocumentsApi => $"{Url}/api/files/goods/documents";

        public static string UserProfileApi => $"{Url}/api/files/user";

        public static string Culture { get; set; } = "sw-KE";
    }
}
