namespace CharityAction.Web.Infrastructure.Tools
{
    using Imgur.API.Authentication.Impl;
    using Imgur.API.Endpoints.Impl;
    using Imgur.API.Enums;

    public class ImgurApi
    {
        private const string ClientIdImgur = "b293abc3122284f";
        private const string ClientSecret = "55ec60ed604c178ba654e0129369230a344aacff";

        public static string GetAuthorizationUrl()
        {
            var client = new ImgurClient(ClientIdImgur);
            var endpoint = new OAuth2Endpoint(client);
            var authorizationUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Token);

            return authorizationUrl;
        }
    }
}
