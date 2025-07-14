namespace InventoryManagement.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
}
