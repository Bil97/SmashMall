using System;

namespace SmashMall.Shared.Services
{
    public class BaseApi
    {
        public static string Url { get; set; }

        private static readonly string v = Environment.CurrentDirectory + "/wwwroot/files";

        private static string currentDirectory = v;

        public static string CurrentDirectory { get => currentDirectory; set => currentDirectory = value; }

        public static string DefaultImage => $"{CurrentDirectory}/default/default-image.png";

        public static string DefaultImageApi => $"api/files/default/image";

        public static string ItemsPhotosDirectory => $"{CurrentDirectory}/images/items";

        public static string ItemsPhotosApi => $"api/files/item";

        public static int ItemsPhotosWidth => 300;
        
        public static int ItemsPhotosHeight => 225;

        public static string ItemsDocumentsDirectory => $"{CurrentDirectory}/documents";

        public static string GoodsDocumentsApi => $"api/files/goods/documents";

        public static string UserProfile => $"{CurrentDirectory}/images/users";

        public static string UserProfileApi => $"api/files/user";

        public static string Culture { get; set; } = "sw-KE";
    }
}
