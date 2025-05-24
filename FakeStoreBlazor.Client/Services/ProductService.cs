using FakeStoreBlazor.Client.Models;
using System.Net.Http.Json;

namespace FakeStoreBlazor.Client.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://fakestoreapi.com/products";

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<List<Product>>(BaseUrl);
        return products ?? new List<Product>();
    }

    public async Task<Product> GetProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<Product>() ??
                throw new InvalidOperationException("Product data is null");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProductAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, product);
        var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
        if (createdProduct == null)
        {
            throw new InvalidOperationException("Failed to create product.");
        }
        return createdProduct;
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", product);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error updating product: {response.StatusCode} - {errorContent}");
            }

            var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();
            return updatedProduct ?? throw new InvalidOperationException("Updated product data is null");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateProductAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/categories");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            var categories = await response.Content.ReadFromJsonAsync<List<string>>();
            return categories ?? new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCategoriesAsync: {ex.Message}");
            throw;
        }
    }
}
