using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class SmtpEmailSettingsViewModel
    {
        [Required(ErrorMessage = "Required Host")]
        public string Host { get; set; }
        [Required(ErrorMessage = "Required Port")]
        public string Port { get; set; }
        [Required(ErrorMessage = "Required Timeout")]
        public string Timeout { get; set; }
        [Required(ErrorMessage = "Required SslProtocol")]
        public string SslProtocol { get; set; }
        [Required(ErrorMessage = "Required TlSProtocol")]
        public string TlSProtocol { get; set; }
        [Required(ErrorMessage = "Required Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Required Password")]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Required Setting Name")]
        [DisplayName("Setting Name")]
        public string Name { get; set; }
        public int? SmtpProviderId { get; set; }
    }

    public class RequestSmtp
    {
        public int? SmtpProviderId { get; set; }
    }
}
