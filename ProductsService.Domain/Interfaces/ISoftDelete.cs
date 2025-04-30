namespace ProductService.Domain.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public void Undo()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}
//For now I'll leave it here I think (I don't know where it needs to be)