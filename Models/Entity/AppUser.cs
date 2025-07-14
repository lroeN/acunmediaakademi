using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Models.Entity;

public class AppUser : IdentityUser
{
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}