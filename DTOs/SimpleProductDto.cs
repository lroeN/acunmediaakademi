namespace InventoryManagement.DTOs;

public class SimpleProductDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
}