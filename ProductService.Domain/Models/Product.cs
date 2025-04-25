using ClientsService.Domain.SoftDelete;

namespace ProductService.Domain.Models;

public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateOnly CreatedOn { get; set; }
    public int UserId { get; set; }
}