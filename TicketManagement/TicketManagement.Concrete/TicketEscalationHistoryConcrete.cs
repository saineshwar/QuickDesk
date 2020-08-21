using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class TicketEscalationHistoryConcrete : ITicketEscalationHistory
    {
        public bool CheckIsTicketAlreadyEscalate(RequestEscalationTicket request)
        {
            using (var db = new DatabaseContext())
            {
                var data = (db.TicketEscalationHistory.Any(t => t.TicketId == request.TicketId));
                return data;
            }
        }
    }
}
