using FluentEmail.Core;
using InventoryManagement.Interfaces;


namespace InventoryManagement.Models.Entity;

public class EmailService(IFluentEmailFactory fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await fluentEmail.Create()
            .To(email)
            .Subject(subject)
            .Body(htmlMessage, true)
            .SendAsync();
    }
}