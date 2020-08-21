using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class DashboardTicketCountConcrete : IDashboardTicketCount
    {
        public DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAgent(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<DashboardCountViewModel>("Usp_Tickets_GetAllTicketbyStatusofCurrentDayforAgent", param
                        , null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAdmin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    return con.Query<DashboardCountViewModel>("Usp_Tickets_GetAllTicketbyStatusofCurrentDayforAdmin", null
                        , null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAgentAdmin(string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    return con.Query<DashboardCountViewModel>("Usp_Tickets_GetAllTicketbyStatusofCurrentDayforAdminWithCategoryId", param
                        , null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforUser(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<DashboardCountViewModel>("Usp_Tickets_GetAllTicketbyStatusofCurrentDayforUser", param
                        , null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
