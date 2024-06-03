using _1.Interfaces;
using Microsoft.AspNetCore.Routing.Template;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _1.Helpers
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(List<string> email, MailMessage msg)
        {
            MailAddress fromEmail = new MailAddress("ibragdi05@gmail.com", "КИИ");
            MailMessage message = new MailMessage()
            {
                Body = msg.Body,
                Subject = msg.Subject,
                From = fromEmail
            };

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(fromEmail.Address, "atgb nnff agjf krll");

            for (int i = 0; i < email.Count; i++)
            {
                message.To.Add(email[i]);
                await smtp.SendMailAsync(message);
            }
            //smtp.SendMailAsync(message);
            #region 
            //var mail = new MailAddress("ibragdi05@gmail.com", "КИИ");
            //var pw = "atgb nnff agjf krll";
            //MailMessage message = new MailMessage(mail, email);
            //message.Body = $"Заявка на посещение объекта КИИ одобрена, дата посещения {date}, время посещения: {request.Time}";
            //message.Subject = "Заявка одобрена";
            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential(mail.Address, pw)
            //};
            //return client.SendAsync();
            #endregion
        
        }

        public async Task SendEmailAsync(string email, MailMessage msg)
        {
            MailAddress fromEmail = new MailAddress("ibragdi05@gmail.com", "КИИ");
            MailMessage message = new MailMessage()
            {
                Body = msg.Body,
                Subject = msg.Subject,
                From = fromEmail
            };

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(fromEmail.Address, "atgb nnff agjf krll");

            message.To.Add(email);
            await smtp.SendMailAsync(message);
        }
    }
}
