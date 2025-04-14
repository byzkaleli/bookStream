// Configurations/EmailSettings.cs
namespace bookStream.Configurations
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587; // SmtpPort yerine Port
        public string Username { get; set; } = string.Empty; // SenderEmail yerine Username
        public string Password { get; set; } = string.Empty; // SenderPassword yerine Password
        public bool UseSsl { get; set; } = true; // EnableSsl yerine UseSsl
        public string FromAddress { get; set; } = string.Empty; // Yeni eklendi
    }
}