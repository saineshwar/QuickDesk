using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("SMTPEmailSettings")]
    public class SmtpEmailSettings
    {
        [Key]
        public int SmtpProviderId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Timeout { get; set; }
        public string SslProtocol { get; set; }
        public string TlSProtocol { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
    }
}
