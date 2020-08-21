using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class ViewTicketReplyHistoryModel
    {
        public long? TicketReplyId { get; set; }
        public string RepliedUserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string TrackingId { get; set; }
        public string Viewcolor { get; set; }
        public long TicketId { get; set; }
        public bool DeleteStatus { get; set; }
        public short RoleId { get; set; }
        public string Note { get; set; }
        public long? SystemUser { get; set; }
        public long? TicketUser { get; set; }
    }
}
