using System.Net.Mail;
using System.Net;

namespace Ecommerce.Services
{
    public class EmailService
    {
            private readonly string _smtpHost = "smtp.gmail.com";
            private readonly int _smtpPort = 587;
            private readonly string _fromEmail = "sandaruj1998@gmail.com";
            private readonly string _fromPassword = "irra mhex kpfw mnyg";

            public async Task SendEmailAsync(string toEmail, string subject, string body)
            {
                var smtpClient = new SmtpClient(_smtpHost)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_fromEmail, _fromPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(toEmail);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw; 
                }
            }
        }
    }
