namespace Application.Constants
{
    public static class ApiRoutes
    {
        private const string Version = "v{version:apiVersion}";

        private const string Base = Version;

        public static class Account
        {
            private const string ControllerName = Base + "/account/";

            public const string Login = ControllerName + "login";

            public const string Register = ControllerName + "register";

            public const string RefreshToken = ControllerName + "refreshToken";

            public const string Roles = ControllerName + "roles";
        }
        
        public static class Article
        {
            private const string ControllerName = Base + "/articles/";

            public const string ArticleId = "{articleId}";

            public const string GetAll = ControllerName;
            
            public const string GetById = ControllerName + ArticleId;

            public const string Create = ControllerName;

            public const string Delete = ControllerName + ArticleId;

            public const string Update = ControllerName + ArticleId;
        }
    }
}