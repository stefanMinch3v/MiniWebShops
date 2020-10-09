namespace CharityAction.Data.Models
{
    using Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Image : BaseModel<int>
    {
        [Required]
        public string ImagePath { get; set; }

        public DateTime DateOfAdded { get; set; }

        public string Description { get; set; }

        public int? EventId { get; set; }
    }
}
