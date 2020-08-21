using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class AgentCheckInStatusConcrete : IAgentCheckInStatus
    {
        private readonly DatabaseContext _context;
        public AgentCheckInStatusConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void StatusCheckInCheckOut(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    var result = con.Execute("USP_AgentCheckInOut", param, transaction, 0, CommandType.StoredProcedure);

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

        public bool CheckIsalreadyCheckedIn(long userId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var checkStatus = (from a in context.AgentCheckInStatusSummary
                                       where a.UserId == userId
                                       select a.AgentStatus).FirstOrDefault();

                    return checkStatus;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AgentDailyActivityModel> AgentDailyActivity(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<AgentDailyActivityModel>("Usp_AgentDailyActivity", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsCategoryAssignedtoAgent(long userId)
        {
            try
            {
                var result = (from menu in _context.AgentCategoryAssigned
                    where menu.UserId == userId
                    select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
