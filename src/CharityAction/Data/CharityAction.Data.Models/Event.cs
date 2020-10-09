namespace CharityAction.Data.Models
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Event : BaseModel<int>
    {
        [Required]
        [MinLength(ModelsConstants.EventMinLength)]
        [MaxLength(ModelsConstants.EventMaxLengthGeneral)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ModelsConstants.EventMaxContentLength)]
        public string Content { get; set; }

        [Required]
        [MinLength(ModelsConstants.EventMinLength)]
        [MaxLength(ModelsConstants.EventMaxLengthGeneral)]
        public string City { get; set; }

        [Required]
        [MinLength(ModelsConstants.EventMinLength)]
        [MaxLength(ModelsConstants.EventMaxLengthGeneral)]
        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(ModelsConstants.EventMaxLengthGeneral)]
        public string BackgroundImageUrl { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}
