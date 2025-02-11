namespace ShoppingCartSW.Models.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllproducs();
        Product GetProductById(int id);
    }
}
