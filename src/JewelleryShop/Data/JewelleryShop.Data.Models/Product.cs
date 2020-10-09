namespace JewelleryShop.Data.Models
{
    using Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseModel<Guid>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PurchaseNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public DateTime DateOfAdded { get; set; }

        public DateTime? DateOfLastModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}
