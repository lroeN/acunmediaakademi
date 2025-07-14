namespace InventoryManagement.DTOs;

public class RegisterDto
{
    public string Email { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}