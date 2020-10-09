namespace JewelleryShop.Web.ViewModels.Products
{
    using Common.Mapping;
    using Microsoft.AspNetCore.Http;
    using Services.Products.Models;
    using System.ComponentModel.DataAnnotations;

    using static WebConstants;

    public class ProductFormViewModel : IMapFrom<ProductDetailsServiceModel>
    {
        [Required(ErrorMessage = TitleError)]
        public string Title { get; set; }

        [Required(ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        [Required(ErrorMessage = PurchaseNumberError)]
        public string PurchaseNumber { get; set; }

        [Required(ErrorMessage = PriceError)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public IFormFile Image { get; set; }

        public bool IsEditModel { get; set; }

        public string ImagePath { get; set; }
    }
}
