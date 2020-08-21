using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class TicketReplyViewModel
    {
        [MaxLength(2000)]
        [AllowHtml]
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }
        public long? TicketId { get; set; }
        public string TrackingId { get; set; }
        public long? TicketReplyId { get; set; }
        public bool HasAttachment { get; set; }
        public string Note { get; set; }
    }
}
