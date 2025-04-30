using ProductService.Domain.Interfaces;

namespace ProductService.Domain.Models;

public class Product : ISoftDelete
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CountInStock { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateOnly CreatedOn { get; set; }
    public int UserId { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}