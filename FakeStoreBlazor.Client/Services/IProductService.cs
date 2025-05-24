using FakeStoreBlazor.Client.Models;

namespace FakeStoreBlazor.Client.Services;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
    Task<List<string>> GetCategoriesAsync();
}
