using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using FakeStoreBlazor.Client.Models;
using FakeStoreBlazor.Client.Services;

namespace FakeStoreBlazor.Client.Pages.Products;

// IMPORTANTE: A classe deve ter o mesmo nome do arquivo .razor (Index)
public partial class Index : ComponentBase
{
    // ============================================================================
    // INJEÇÃO DE DEPENDÊNCIAS
    // ============================================================================

    [Inject] protected IProductService ProductService { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

    // ============================================================================
    // PROPRIEDADES/ESTADO DO COMPONENTE
    // ============================================================================

    private List<Product>? products;
    private bool isLoading = true;

    // ============================================================================
    // LIFECYCLE - MÉTODOS DO CICLO DE VIDA
    // ============================================================================

    protected override async Task OnInitializedAsync()
    {
        try
        {
            products = await ProductService.GetProductsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            await JSRuntime.InvokeVoidAsync("console.error", $"Error loading products: {ex.Message}");
        }
        finally
        {
            isLoading = false;
        }
    }

    // ============================================================================
    // MÉTODOS DE NAVEGAÇÃO
    // ============================================================================

    private void NavigateToAdd() => Navigation.NavigateTo("/products/add");

    private void ViewDetails(int id) => Navigation.NavigateTo($"/products/{id}");

    private void EditProduct(int id) => Navigation.NavigateTo($"/products/edit/{id}");

    // ============================================================================
    // MÉTODOS DE AÇÃO
    // ============================================================================

    private async Task DeleteProduct(int id)
    {
        try
        {
            // Confirma com o usuário
            var confirm = await JSRuntime.InvokeAsync<bool>("confirm",
                $"Deseja realmente excluir o produto #{id}?");

            if (confirm)
            {
                // Exclui o produto
                await ProductService.DeleteProductAsync(id);

                // Recarrega a lista
                products = await ProductService.GetProductsAsync();

                // Força o Blazor a atualizar a tela
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            await JSRuntime.InvokeVoidAsync("alert", "Erro ao excluir produto");
        }
    }
}