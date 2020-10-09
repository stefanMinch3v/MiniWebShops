namespace JewelleryShop.Services.External.Implementations
{
    using Imgur.API;
    using Imgur.API.Authentication.Impl;
    using Imgur.API.Endpoints.Impl;
    using Imgur.API.Models;
    using Imgur.API.Models.Impl;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class ImgurApiService : IImgurApiService
    {
        private const string ClientId = "52e2522ba76e7d2";
        private const string ClientSecret = "91ad988e6a05b96dc7238cfb0b137895e6d29fa5";

        public async Task<string> UploadImageAsync(IFormFile fileImage, string imgurToken)
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
            catch (ImgurException imgurEx)
            {
               throw new InvalidOperationException(imgurEx.Message);
            }
        }
    }
}
