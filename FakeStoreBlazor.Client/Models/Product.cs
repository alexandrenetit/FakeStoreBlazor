using System.ComponentModel.DataAnnotations;

namespace FakeStoreBlazor.Client.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Price { get; set; }

    [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [StringLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres")]
    public string? Category { get; set; }

    [Url(ErrorMessage = "Insira uma URL válida para a imagem")]
    public string? Image { get; set; }
}