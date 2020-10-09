namespace JewelleryShop.Services.Products.Models
{
    using Common.Mapping;
    using Data.Models;
    using System;

    public class ProductDetailsServiceModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PurchaseNumber { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateOfAdded { get; set; }

        public DateTime? DateOfLastModified { get; set; }
    }
}
