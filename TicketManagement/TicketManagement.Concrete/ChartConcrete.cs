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
    public class ChartConcrete : IChart
    {
        public List<ChartViewModel> GetListofBarChartData(long userid)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userid);
                    return con.Query<ChartViewModel>("Usp_GetBarChartByUserId_Agent", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ChartViewModel> GetListofBarChartDataAdmin()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    
                    return con.Query<ChartViewModel>("Usp_GetBarChartByUserId_Admin", null, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ChartViewModel> GetListofBarChartDataAgentsAdmin(int categoryId)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    return con.Query<ChartViewModel>("Usp_GetBarChartByUserId_AgentsAdmin", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DonutChartViewModel> GetListofDonutChartData(long userid)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userid);
                    return con.Query<DonutChartViewModel>("Usp_GetDonutChartByUserId_Agent", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DonutChartViewModel> GetListofDonutChartDataAdmin()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {

                    return con.Query<DonutChartViewModel>("Usp_GetDonutChartByUserId_Admin", null, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DonutChartViewModel> GetListofDonutChartDataAgentsAdmin(int categoryId)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    return con.Query<DonutChartViewModel>("Usp_GetDonutChartByUserId_AgentsAdmin", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
