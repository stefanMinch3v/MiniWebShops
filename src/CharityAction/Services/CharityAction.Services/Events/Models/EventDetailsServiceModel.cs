namespace CharityAction.Services.Events.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Common;
    using Data.Models;
    using Images.Models;
    using System;
    using System.Collections.Generic;

    public class EventDetailsServiceModel : BaseModel<int>, IMapFrom<Event>, IHaveCustomMapping
    {
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public string City { get; set; }
        
        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<ImageDetailsServiceModel> Images { get; set; } = new List<ImageDetailsServiceModel>();

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Event, EventDetailsServiceModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(cfg => cfg.BackgroundImageUrl));

            mapper.CreateMap<Image, ImageDetailsServiceModel>();
        }
    }
}
