namespace CharityAction.Common.Tools
{
    using Imgur.API;
    using Imgur.API.Authentication.Impl;
    using Imgur.API.Endpoints.Impl;
    using Imgur.API.Enums;
    using Imgur.API.Models;
    using Imgur.API.Models.Impl;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class ImgurApiClient
    {
        private const string ClientId = "b293abc3122284f";
        private const string ClientSecret = "b2fbc341e20d0029c5766fe5a709ebb217488119";

        /// <summary>
        /// Uploads image to imgur and returns the display url
        /// </summary>
        /// <param name="fileImage"></param>
        /// <param name="imgurToken"></param>
        /// <returns></returns>
        public static async Task<string> UploadImageAsync(IFormFile fileImage, string imgurToken)
        {
            try
            {
                var token = new OAuth2Token(imgurToken, "", "", "", "", 0);
                var client = new ImgurClient(ClientId, ClientSecret, token);
                var endpoint = new ImageEndpoint(client);

                IImage image;

                using (var ms = new MemoryStream())
                {
                    fileImage.CopyTo(ms);
                    image = await endpoint.UploadImageBinaryAsync(ms.ToArray());
                }

                return image.Link;
            }
            catch (ImgurException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch(MashapeException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Redirects to imgur in order to validate the access token
        /// </summary>
        /// <returns></returns>
        public static string GetAuthorizationUrl()
        {
            var client = new ImgurClient(ClientId);
            var endpoint = new OAuth2Endpoint(client);
            var authorizationUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Token);

            return authorizationUrl;
        }
    }
}
