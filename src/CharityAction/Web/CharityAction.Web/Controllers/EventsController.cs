namespace CharityAction.Web.Controllers
{
    using Common.Tools;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Events;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using ViewModels;

    [Authorize(Roles = WebConstants.AdministratorRole)]
    public class EventsController : Controller
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var events = await this.eventService.GetAllAsync(page, WebConstants.PageSize);

            if (page > 1)
            {
                return this.Json(events);
            }

            return this.View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.TempData.AddErrorMessage(WebConstants.InvalidEventCreation);
                return this.View();
            }

            try
            {
                var imgurToken = this.GetImgurToken();

                await this.eventService.AddAsync(
                    imgurToken,
                    model.Title,
                    model.Content,
                    model.City,
                    model.Address,
                    model.Image,
                    model.ImageUrl,
                    model.StartDate,
                    model.EndDate);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Create));
            }
            catch (ArgumentNullException ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Create));
            }

            this.TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullyAddedEvent, model.Title));
            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var currentEvent = await this.eventService.GetByIdAsync(id);

                if (currentEvent == null)
                {
                    return this.BadRequest();
                }

                return this.View(currentEvent);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int id, string eventTitle)
        {
            CoreValidator.ThrowIfInvalidIntegerProvided(id, nameof(id));

            try
            {
                await this.eventService.DeleteAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullyDeletedEvent, eventTitle));
            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var currentEvent = await this.eventService.GetByIdAsync(id);

                if (currentEvent == null)
                {
                    return this.BadRequest();
                }

                var eventFormModel = AutoMapper.Mapper.Map<EventFormViewModel>(currentEvent);
                return this.View(eventFormModel);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventFormViewModel model)
        {
            try
            {
                await this.eventService.EditAsync(
                    id,
                    model.Title,
                    model.Content,
                    model.City,
                    model.Address,
                    model.ImageUrl,
                    model.StartDate,
                    model.EndDate);

                this.TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullyAddedEvent, model.Title));
                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Edit), new { id });
            }
            catch (Exception ex)
            {
                this.TempData.AddErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(Edit), new { id });
            }
        }

        public IActionResult LoginImgurProfile()
        {
            var authorizationUrl = ImgurApiClient.GetAuthorizationUrl();
            return this.Redirect(authorizationUrl);
        }

        private string GetImgurToken()
        {
            this.HttpContext.Request.Cookies.TryGetValue("MySecurityToken", out string imgurToken);

            if (string.IsNullOrEmpty(imgurToken))
            {
                return string.Empty;
            }

            imgurToken = this.DecodeTokenFromBase64(imgurToken);

            return imgurToken;
        }

        private string DecodeTokenFromBase64(string imgurToken)
        {
            var data = Convert.FromBase64String(imgurToken);
            return Encoding.UTF8.GetString(data);
        }

        private EventFormViewModel GetEventFormWithErrors(EventFormViewModel model)
        {
            return new EventFormViewModel
            {
                Address = model.Address,
                City = model.City,
                Content = model.Content,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                Title = model.Title
            };
        }
    }
}
