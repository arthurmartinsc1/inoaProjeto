

namespace inoaProjectx.Models
{
        public class EmailSettings
    {
        public string Destinatario { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 0;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Destinatário: {Destinatario}\n" +
                   $"SMTP Host: {SmtpHost}\n" +
                   $"SMTP Port: {SmtpPort}\n" +
                   $"Usuário: {Usuario}\n" +
                   $"Senha: {(string.IsNullOrEmpty(Senha) ? "não configurada" : "****")}";
        }
    }
}