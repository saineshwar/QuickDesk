using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class TicketNotificationConcrete : ITicketNotification
    {
        public List<TicketNotificationViewModel> ListofNotification(long? agentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@AgentId", agentId);
                    return con.Query<TicketNotificationViewModel>("Usp_GetTicketNotificationbyAgentId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? GetNotificationCount(long? agentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@AgentId", agentId);
                    return con.Query<int?>("Usp_GetTicketNotificationCountbyAgentId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateTicketNotificationasRead(long? agentId, long? notificationId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@NotificationId", notificationId);
                    param.Add("@AgentId", agentId);
                    var result = con.Execute("Usp_UpdateTicketNotificationasRead", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}