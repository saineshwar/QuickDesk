using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class ViewTicketReplyMainModel
    {
        public List<ViewTicketReplyHistoryModel> ListofTicketreply { get; set; }
        public List<ReplyAttachment> ListofReplyAttachment { get; set; }

    }
}
