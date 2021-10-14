using System;
using System.Threading.Tasks;
using CryptoAPI.Interfaces;
using CryptoAPI.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CryptoAPI.Services
{
    public class EmailService : IEmailService
    {
        EmailSettings _emailSettings = null;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public async Task<string> SendEmail(EmailData emailData)
        {
            Console.WriteLine("Podaci: " + _emailSettings.Password + " - " + _emailSettings.Host + " - " + _emailSettings.EmailId);
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient emailClient = new SmtpClient();
                if (!emailClient.IsConnected)
                {
                    emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                }
                
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                Console.WriteLine("Poslano!");
                return "Sent";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! + " + ex.Message + ex.StackTrace);
                return ex.Message + ex.StackTrace;
            }
        }
    }
}
