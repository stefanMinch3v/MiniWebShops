namespace CharityAction.Services.Events
{
    using Microsoft.AspNetCore.Http;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventService
    {
        Task AddAsync(
            string imgurToken,
            string title,
            string content,
            string city,
            string address,
            IFormFile image,
            string imageUrl,
            DateTime startDate,
            DateTime? endDate);

        Task<EventDetailsServiceModel> GetByIdAsync(int id);

        Task<IReadOnlyCollection<EventListsServiceModel>> GetAllAsync(int page, int pageSize);

        Task DeleteAsync(int id);

        Task EditAsync(
            int id,
            string title,
            string content,
            string city,
            string address,
            string imageUrl,
            DateTime startDate,
            DateTime? endDate);
    }
}
