namespace CharityAction.Services.Images
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task AddAsync(IEnumerable<IFormFile> images, string imgurToken, int? eventId);
    }
}
