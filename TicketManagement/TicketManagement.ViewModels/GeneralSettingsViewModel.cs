using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class GeneralSettingsViewModel
    {
        public int? GeneralSettingsId { get; set; }
        [Required(ErrorMessage = "Required From Email")]
        [DisplayName("From Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required From Name")]
        [DisplayName("From Name")]
        public string Name { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string SupportEmailId { get; set; }
        public string WebsiteTitle { get; set; }
        public string WebsiteUrl { get; set; }
        [DisplayName("Enable Email Feature")]
        public bool EnableEmailFeature { get; set; }
        [DisplayName("Enable SMS Feature")]
        public bool EnableSmsFeature { get; set; }
        [DisplayName("Enable Signature Feature")]
        public bool EnableSignatureFeature { get; set; }
    }
}
