namespace CharityAction.Services.Events
{
    using AutoMapper.QueryableExtensions;
    using Common.Tools;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventService : IEventService
    {
        private readonly CharityDbContext dbContext;

        public EventService(CharityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(
            string imgurToken,
            string title,
            string content,
            string city,
            string address,
            IFormFile image,
            string imageUrl,
            DateTime startDate,
            DateTime? endDate)
        {
            this.ValidateEventData(title, content, city, address, startDate, endDate);

            var imageLink = string.Empty;

            if (image != null)
            {
                imageLink = await ImgurApiClient.UploadImageAsync(image, imgurToken);
            }
            else
            {
                imageLink = imageUrl;
            }

            var newEvent = new Event
            {
                Address = address,
                City = city,
                Content = content,
                EndDate = endDate.HasValue ? endDate.Value.ToUniversalTime() : endDate,
                StartDate = startDate.ToUniversalTime(),
                Title = title,
                BackgroundImageUrl = imageLink
            };

            this.dbContext.Events.Add(newEvent);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<EventListsServiceModel>> GetAllAsync(int page = 1, int pageSize = 8)
        {
            CoreValidator.ThrowIfInvalidIntegerProvided(page, nameof(page));
            CoreValidator.ThrowIfInvalidIntegerProvided(pageSize, nameof(pageSize));

            return await this.dbContext.Events
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<EventListsServiceModel>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EventDetailsServiceModel> GetByIdAsync(int id)
        {
            CoreValidator.ThrowIfInvalidIntegerProvided(id, nameof(id));

            return await this.dbContext.Events
                .Where(e => e.Id == id && e.IsDeleted == false)
                .ProjectTo<EventDetailsServiceModel>()
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            CoreValidator.ThrowIfInvalidIntegerProvided(id, nameof(id));

            var existingEvent = await this.dbContext.Events.FindAsync(id);

            if (existingEvent == null)
            {
                throw new InvalidOperationException(string.Format(ServiceConstants.UnexistingEvent, id));
            }

            existingEvent.IsDeleted = true;
            this.dbContext.Events.Update(existingEvent);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(
            int id,
            string title, 
            string content, 
            string city, 
            string address, 
            string imageUrl, 
            DateTime startDate, 
            DateTime? endDate)
        {
            ValidateEventData(title, content, city, address, startDate, endDate);

            var existingEvent = await this.dbContext.Events.FindAsync(id);

            if (existingEvent == null)
            {
                throw new InvalidOperationException(string.Format(ServiceConstants.UnexistingEvent, id));
            }

            existingEvent.Title = title;
            existingEvent.Content = content;
            existingEvent.City = city;
            existingEvent.Address = address;
            existingEvent.StartDate = startDate.ToUniversalTime();
            existingEvent.EndDate = endDate.HasValue ? endDate.Value.ToUniversalTime() : endDate;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                existingEvent.BackgroundImageUrl = imageUrl;
            }

            this.dbContext.Events.Update(existingEvent);

            await this.dbContext.SaveChangesAsync();
        }

        private void ValidateEventData(
            string title,
            string content,
            string city,
            string address,
            DateTime startDate,
            DateTime? endDate)
        {
            CoreValidator.ThrowIfNullOrEmpty(title, nameof(title));
            CoreValidator.ThrowIfNullOrEmpty(content, nameof(content));
            CoreValidator.ThrowIfNullOrEmpty(city, nameof(city));
            CoreValidator.ThrowIfNullOrEmpty(address, nameof(address));

            var minimumDate = DateTime.Now.AddYears(-30);

            if (title.Length < 3)
            {
                throw new InvalidOperationException(nameof(title));
            }

            if (content.Length > 5000)
            {
                throw new InvalidOperationException(nameof(content));
            }

            if (city.Length < 3)
            {
                throw new InvalidOperationException(nameof(city));
            }

            if (address.Length < 3)
            {
                throw new InvalidOperationException(nameof(address));
            }

            if (startDate <= minimumDate)
            {
                throw new InvalidOperationException(nameof(startDate));
            }

            if (endDate.HasValue && endDate.Value <= minimumDate)
            {
                throw new InvalidOperationException(nameof(endDate));
            }
        }
    }
}
