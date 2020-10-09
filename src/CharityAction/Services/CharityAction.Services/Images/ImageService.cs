namespace CharityAction.Services.Images
{
    using Common.Tools;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        private readonly CharityDbContext dbContext;

        public ImageService(CharityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(
            IEnumerable<IFormFile> images,
            string imgurToken,
            int? eventId)
        {
            Event existingEvent = null;

            if (eventId != null)
            {
                existingEvent = await this.dbContext.Events.FindAsync(eventId);

                if (existingEvent == null)
                {
                    throw new InvalidOperationException(ServiceConstants.UnexistingEvent);
                }
            }

            foreach (var formFile in images)
            {
                var imagePath = await ImgurApiClient.UploadImageAsync(formFile, imgurToken);

                var image = new Image
                {
                    ImagePath = imagePath,
                    DateOfAdded = DateTime.UtcNow,
                    EventId = eventId
                };

                if (existingEvent != null)
                {
                    existingEvent.Images.Add(image);
                }
                else
                {
                    await this.dbContext.Images.AddAsync(image);
                }
            }

            if (existingEvent != null)
            {
                this.dbContext.Events.Update(existingEvent);
            }

            await this.dbContext.SaveChangesAsync();
        }
    }
}
