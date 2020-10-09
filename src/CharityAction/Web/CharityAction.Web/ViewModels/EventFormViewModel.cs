namespace CharityAction.Web.ViewModels
{
    using Common.Mapping;
    using Microsoft.AspNetCore.Http;
    using Services.Events.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EventFormViewModel : IMapFrom<EventDetailsServiceModel>
    {
        [Required]
        [MinLength(WebConstants.EventMinLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(WebConstants.EventMaxContentLength)]
        public string Content { get; set; }

        [Required]
        [MinLength(WebConstants.EventMinLength)]
        public string City { get; set; }

        [Required]
        [MinLength(WebConstants.EventMinLength)]
        public string Address { get; set; }

        // optional to add image from PC/phone or url
        [Display(Name = "Upload image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Add image url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }
    }
}
