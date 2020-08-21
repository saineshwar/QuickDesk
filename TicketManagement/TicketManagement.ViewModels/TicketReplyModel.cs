using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class TicketReplyModel
    {
        public string TrackingId { get; set; }
        public long? TicketId { get; set; }

        [StringLength(700)]
        [AllowHtml]
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

    }
}
