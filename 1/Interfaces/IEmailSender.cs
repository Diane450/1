using System.Net.Mail;

namespace _1.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(List<string> email, MailMessage msg);
    }
}
