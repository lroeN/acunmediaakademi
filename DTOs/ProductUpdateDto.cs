namespace InventoryManagement.DTOs;

public class ProductUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public Guid InventoryId { get; set; }
}