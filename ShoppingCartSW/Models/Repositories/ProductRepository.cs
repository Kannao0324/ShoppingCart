
using ShoppingCartSW.Models.Data;

namespace ShoppingCartSW.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Create a readonly field to hold a reference to our context class
        private readonly ShoppingCartDBContext _context;
        // Request the context class by naming it as a parameter in the constructor
        public ProductRepository(ShoppingCartDBContext context)
        {
            _context = context;
        }
        public List<Product> GetAllproducs()
        {
            // Get all the product from the database 
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            // ?? null-coalesing if return is null, create a blank list
            return _context.Products.Where(p => p.Id == id)
                                 .FirstOrDefault() ?? new Product();
        }
    }
}
