namespace JewelleryShop.Web.Controllers
{
    using AutoMapper;
    using Imgur.API.Authentication.Impl;
    using Imgur.API.Endpoints.Impl;
    using Imgur.API.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Services.Products;
    using Services.Products.Models;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using ViewModels.Products;

    using static WebConstants;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var product = await this.productService.GetSingleAsync(id);
            return this.View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View(new ProductFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var imgurToken = this.ParseImgurToken();

            if (string.IsNullOrEmpty(imgurToken))
            {
                this.TempData[TempDataErrorMessageKey] = InvalidImgurToken;
                return this.RedirectToAction(nameof(Create));
            }

            try
            {
                await this.productService.CreateAsync(
                    model.Title,
                    model.Description,
                    model.PurchaseNumber,
                    model.Price,
                    model.Image,
                    imgurToken);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData[TempDataErrorMessageKey] = ex.Message;
                return this.RedirectToAction(nameof(Create));
            }
            catch(Exception ex)
            {
                this.TempData[TempDataErrorMessageKey] = ex.Message;
                return this.RedirectToAction(nameof(Create));
            }
            
            this.TempData[TempDataSuccessMessageKey] = SuccessfullyAddedProduct;
            return this.RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var existingProduct = await this.productService.GetSingleAsync(id);

            if (existingProduct == null)
            {
                return this.BadRequest(UnexistingProduct);
            }

            var mappedProduct = Mapper.Map<ProductDetailsServiceModel, ProductFormViewModel>(existingProduct);
            mappedProduct.IsEditModel = true;

            return this.View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProductFormViewModel model)
        {
            var isProductExisting = await this.productService.IsProductExistingAsync(id);

            if (!isProductExisting)
            {
                return this.BadRequest(UnexistingProduct);
            }

            var imgurToken = this.ParseImgurToken();

            if (string.IsNullOrEmpty(imgurToken))
            {
                this.TempData[TempDataErrorMessageKey] = InvalidImgurToken;
                return this.RedirectToAction(nameof(Edit), model);
            }

            try
            {
                await this.productService.EditAsync(
                    id, 
                    model.Title,
                    model.Description,
                    model.PurchaseNumber,
                    model.Price,
                    model.Image,
                    model.ImagePath,
                    imgurToken);
            }
            catch (Exception ex)
            {
                this.TempData[TempDataErrorMessageKey] = ex.Message;
                return this.RedirectToAction(nameof(Edit), model);
            }

            this.TempData[TempDataSuccessMessageKey] = SuccessfullyEditedProduct;
            return this.RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var successfullyDeleted = await this.productService.MarkAsDeletedAsync(id);

            if (!successfullyDeleted)
            {
                return this.BadRequest(UnexistingProduct);
            }

            this.TempData[TempDataSuccessMessageKey] = SuccessfullyDeletedProduct;
            return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
        }

        public IActionResult LoginImgurProfile()
        {
            var client = new ImgurClient(ClientIdImgur);
            var endpoint = new OAuth2Endpoint(client);
            var authorizationUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Token);

            return this.Redirect(authorizationUrl);
        }

        private string ParseImgurToken()
        {
            var imgurToken = string.Empty;
            this.HttpContext.Request.Cookies.TryGetValue("MySecurityToken", out imgurToken);

            if (string.IsNullOrEmpty(imgurToken))
            {
                return string.Empty;
            }

            imgurToken = this.DecodeTokenFromBase64(imgurToken);

            return imgurToken;
        }

        private string DecodeTokenFromBase64(string imgurToken)
        {
            byte[] data = Convert.FromBase64String(imgurToken);
            return Encoding.UTF8.GetString(data);
        }
    }
}
