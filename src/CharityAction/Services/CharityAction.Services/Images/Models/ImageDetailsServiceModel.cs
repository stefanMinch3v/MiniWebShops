namespace CharityAction.Services.Images.Models
{
    using Common.Mapping;
    using Data.Common;
    using Data.Models;
    using System;

    public class ImageDetailsServiceModel : BaseModel<int>, IMapFrom<Image>
    {
        public string ImagePath { get; set; }

        public DateTime DateOfAdded { get; set; }

        public string Description { get; set; }

        public int? EventId { get; set; }
    }
}
