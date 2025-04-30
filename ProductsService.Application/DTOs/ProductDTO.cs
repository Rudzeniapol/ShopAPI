namespace ProductService.Application.DTOs;

public record ProductDTO
{
    public string ProductName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public bool IsAvailable { get; init; }
    public int CountInStock { get; init; }
}