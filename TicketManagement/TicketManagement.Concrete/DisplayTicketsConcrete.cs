using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class DisplayTicketsConcrete : IDisplayTickets
    {
        private readonly DatabaseContext _context;
        public DisplayTicketsConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public DisplayTicketViewModel TicketsDetailsbyticketId(string trackingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<DisplayTicketViewModel>("Usp_Tickets_TicketsDetailsbyticketId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckTrackingIdExists(string trackingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<bool>("Usp_CheckTrackingIdExists", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}