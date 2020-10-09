namespace JewelleryShop.Services.Products.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using JewelleryShop.Services.External;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ServiceConstants;

    public class ProductService : IProductService
    {
        private readonly ShopDbContext dbContext;
        private readonly IImgurApiService imgurService;

        public ProductService(ShopDbContext dbContext, IImgurApiService imgurService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.imgurService = imgurService ?? throw new ArgumentNullException(nameof(imgurService));
        }

        public async Task CreateAsync(
            string title,
            string description,
            string purchaseNumber,
            decimal price,
            IFormFile image,
            string imgurToken)
        {
            this.ValidateCreateProductData(title, description, purchaseNumber, price, image);

            var imageLink = await this.imgurService.UploadImageAsync(image, imgurToken);

            var product = new Product
            {
                Title = title,
                Description = description,
                PurchaseNumber = purchaseNumber,
                Price = price,
                ImagePath = imageLink,
                DateOfAdded = DateTime.UtcNow
            };

            await this.dbContext.Products.AddAsync(product);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(
            string id,
            string title,
            string description,
            string purchaseNumber,
            decimal price,
            IFormFile image,
            string imagePath,
            string imgurToken)
        {
            var hasUploadedPicture = this.ValidateEditProductDataAndHasUploadedImage(
                title, 
                description, 
                purchaseNumber, 
                price, 
                image, 
                imagePath);

            if (hasUploadedPicture)
            {
                imagePath = await this.imgurService.UploadImageAsync(image, imgurToken);
            }

            var existingProduct = await this.dbContext.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (existingProduct == null)
            {
                throw new InvalidOperationException(UnexistingProduct);
            }

            existingProduct.ImagePath = imagePath;
            existingProduct.Price = price;
            existingProduct.PurchaseNumber = purchaseNumber;
            existingProduct.Title = title;
            existingProduct.Description = description;
            existingProduct.DateOfLastModified = DateTime.UtcNow;

            this.dbContext.Products.Update(existingProduct);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductListingServiceModel>> GetAllAsync()
            => await this.dbContext.Products
                .OrderBy(p => p.DateOfAdded)
                .ProjectTo<ProductListingServiceModel>()
                .ToListAsync();

        public async Task<ProductDetailsServiceModel> GetSingleAsync(string id)
            => await this.dbContext.Products
                .Where(p => p.Id.ToString() == id)
                .ProjectTo<ProductDetailsServiceModel>()
                .FirstOrDefaultAsync();

        public async Task<bool> MarkAsDeletedAsync(string id)
        {
            var product = await this.dbContext.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (product == null)
            {
                return false;
            }

            product.IsDeleted = true;

            this.dbContext.Products.Update(product);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsProductExistingAsync(string id)
            => await this.dbContext.Products.AnyAsync(p => p.Id.ToString() == id);

        private void ValidateCreateProductData(
            string title,
            string description,
            string purchaseNumber,
            decimal price,
            IFormFile image)
        {
            if (string.IsNullOrEmpty(title)
                || string.IsNullOrEmpty(description)
                || string.IsNullOrEmpty(purchaseNumber)
                || image == null
                || (!(image.FileName.ToLower().EndsWith(".jpg") || image.FileName.ToLower().EndsWith(".png") || image.FileName.ToLower().EndsWith(".jpeg")))
                || price < 0)

            {
                throw new InvalidOperationException(InvalidInsertedProductData);
            }
        }

        private bool ValidateEditProductDataAndHasUploadedImage(
            string title,
            string description,
            string purchaseNumber,
            decimal price,
            IFormFile image,
            string imagePath)
        {
            var hasUploadedImage = false;

            if (string.IsNullOrEmpty(title)
                || string.IsNullOrEmpty(description)
                || string.IsNullOrEmpty(purchaseNumber)
                || price < 0)

            {
                throw new InvalidOperationException(InvalidInsertedProductData);
            }

            if (image != null)
            {
                if (!(image.FileName.ToLower().EndsWith(".jpg")
                    || image.FileName.ToLower().EndsWith(".png")
                    || image.FileName.ToLower().EndsWith(".jpeg")))
                {
                    throw new InvalidOperationException(InvalidInsertedProductData);
                }

                hasUploadedImage = true;
            }
            else if(!string.IsNullOrEmpty(imagePath))
            {
                var existingImageUrl = this.dbContext.Products.Any(p => p.ImagePath == imagePath);

                if (!existingImageUrl)
                {
                    throw new InvalidOperationException(InvalidInsertedProductData);
                }
            }

            return hasUploadedImage;
        }
    }
}
