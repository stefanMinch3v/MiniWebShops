namespace JewelleryShop.Web.Controllers
{
    using Services.Products;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using ViewModels;
    using System;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public async Task<IActionResult> Index()
        {
            var products = await this.productService.GetAllAsync();
            return this.View(products);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
