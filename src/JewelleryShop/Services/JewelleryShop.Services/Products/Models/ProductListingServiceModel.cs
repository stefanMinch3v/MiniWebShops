namespace JewelleryShop.Services.Products.Models
{
    using Common.Mapping;
    using Data.Models;
    using System;

    public class ProductListingServiceModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public DateTime DateOfAdded { get; set; }
    }
}
