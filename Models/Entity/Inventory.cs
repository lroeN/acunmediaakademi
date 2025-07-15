namespace InventoryManagement.Models.Entity;

public class Inventory
{
    public Guid InventoryId { get; set; }
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public string UserId { get; set; }
    public AppUser User { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
    
    public Guid? ParentInventoryId { get; set; } 
    public Inventory? ParentInventory { get; set; }
    
    public ICollection<Inventory> ChildInventories { get; set; } = new List<Inventory>();
}