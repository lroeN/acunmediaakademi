namespace InventoryManagement.Models.Entity;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    
    public Guid InventoryId{ get; set; }
    public Inventory Inventory { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}