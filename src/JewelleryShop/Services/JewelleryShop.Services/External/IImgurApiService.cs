namespace JewelleryShop.Services.External
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImgurApiService
    {
        Task<string> UploadImageAsync(IFormFile image, string imgurToken);
    }
}