namespace InventoryManagement.DTOs;

public class InventoryCreateDto
{
    public string Name { get; set; } = null!;
    public Guid? ParentInventoryId { get; set; }
}