namespace SmashMall.Shared.Services
{
    public class BaseApi
    {
        public string Url => "https://localhost:44373";

        public string DefaultImage => "wwwroot/files/default/default-image.png";

        public string DefaultImageApi => $"{ Url }/api/files/default/image";

        public string GoodsPhotosDirectory => "wwwroot/files/images/goods";

        public string GoodsPhotosApi => $"{ Url }/api/files/goods";

        public string UserProfile => "wwwroot/files/images/users";

        public string UserProfileApi => $"{ Url }/api/files/user";
    }
}
