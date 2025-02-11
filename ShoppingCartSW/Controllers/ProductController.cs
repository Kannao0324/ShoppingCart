using Microsoft.AspNetCore.Mvc;
using ShoppingCartSW.Models.Repositories;

namespace ShoppingCartSW.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepo)
        {
            _productRepository = productRepo;
        }

        public IActionResult Index()
        {
            //Request all the books from the repository
            var products = _productRepository.GetAllproducs();
            //Pass the books to the view to generate the webpage.
            return View(products);
            
        }
    }
}
