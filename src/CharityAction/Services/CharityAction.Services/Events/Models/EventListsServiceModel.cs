namespace CharityAction.Services.Events.Models
{
    using Data.Models;
    using Common.Mapping;
    using System;

    public class EventListsServiceModel : IMapFrom<Event>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string BackgroundImageUrl { get; set; }
    }
}
