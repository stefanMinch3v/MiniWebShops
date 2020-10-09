namespace JewelleryShop.Services.Products
{
    using Microsoft.AspNetCore.Http;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task CreateAsync(
            string title, 
            string description,
            string purchaseNumber, 
            decimal price,
            IFormFile image,
            string imgurToken);

        Task EditAsync(
            string id,
            string title,
            string description,
            string purchaseNumber,
            decimal price,
            IFormFile image,
            string imagePath,
            string imgurToken);

        Task<IEnumerable<ProductListingServiceModel>> GetAllAsync();

        Task<ProductDetailsServiceModel> GetSingleAsync(string id);

        Task<bool> MarkAsDeletedAsync(string id);

        Task<bool> IsProductExistingAsync(string id);
    }
}
