namespace InventoryManagement.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public Guid InventoryId { get; set; }
}