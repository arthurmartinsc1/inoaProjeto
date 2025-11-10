using inoaProjectx.Models;
using System.Net.Mail;
using System.Net;
using System;

namespace inoaProjectx.Services
{
    public class EmailSvc
    {
        private readonly EmailSettings _emailSettings;
        
        public EmailSvc(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmail(string subject, string body)
        {
            try
            {
                using var mail = new MailMessage();
                mail.From = new MailAddress(_emailSettings.Usuario);
                mail.To.Add(_emailSettings.Destinatario);
                mail.Subject = subject;
                mail.Body = body;

                using var client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
                client.Credentials = new NetworkCredential(_emailSettings.Usuario, _emailSettings.Senha);
                client.EnableSsl = true;
                client.Timeout = 30000;

                await client.SendMailAsync(mail);
                Console.WriteLine("Email enviado com sucesso! aguarde 50 segundos para buscar a próxima cotação...");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($" Erro ao enviar email (SMTP): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erro inesperado ao enviar email: {ex.Message}");
                throw;
            }
        }
    }
}