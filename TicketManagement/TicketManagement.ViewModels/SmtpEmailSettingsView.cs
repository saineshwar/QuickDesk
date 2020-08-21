using System;

namespace TicketManagement.ViewModels
{
    public class SmtpEmailSettingsView
    {
        public long SmtpProviderId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Timeout { get; set; }
        public string SslProtocol { get; set; }
        public string TlSProtocol { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}