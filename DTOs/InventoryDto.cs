namespace InventoryManagement.DTOs;

public class InventoryDto
{
    public Guid InventoryId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UserId { get; set; }
    
    public Guid? ParentInventoryId { get; set; }
    
    public List<InventoryDto> ChildInventories { get; set; } = new List<InventoryDto>();
    
    public List<ProductDto> Products { get; set; }
}