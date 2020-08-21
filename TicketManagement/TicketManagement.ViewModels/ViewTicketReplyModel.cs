using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ViewTicketReplyModel
    {
        public int TicketReplyId { get; set; }
        public long? UserId { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public long? TicketId { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(20)]
        public string CreatedDateDisplay { get; set; }
    }
}
