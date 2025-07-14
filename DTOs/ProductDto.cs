namespace InventoryManagement.DTOs;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public Guid InventoryId { get; set; }
    public string InventoryName { get; set; } = string.Empty;
}