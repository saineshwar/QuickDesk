using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class ExportReportConcrete : IExportReport
    {
        public List<SelectListItem> GetAllAgentListByCategoryId(string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    var agentList = con.Query<SelectListItem>("Usp_GetAllAgentList", param, null, false, 0, CommandType.StoredProcedure).ToList();
                    agentList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    return agentList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllAgentandAgentAdminListByCategoryId(string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    var agentList = con.Query<SelectListItem>("Usp_GetAllAgenAdmintList", param, null, false, 0, CommandType.StoredProcedure).ToList();
                    agentList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    return agentList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CategoryExportReportViewModel> GetCategoryWiseTicketStatusReport(string fromdate, string todate, string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<CategoryExportReportViewModel>("Usp_Report_CategoryWiseTicketStatus", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AgentAdminExportReportViewModel> GetDetailTicketStatusReport(string fromdate, string todate,string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@UserID", userId);
                    return con.Query<AgentAdminExportReportViewModel>("Usp_Report_DetailTicketStatus", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TicketOverduesViewModel> GetTicketOverduesbyCategoryReport(string fromdate, string todate, string overdueTypeId, string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@CategoryId", categoryId);
                    param.Add("@OverdueTypeId", overdueTypeId);
                    return con.Query<TicketOverduesViewModel>("Usp_Report_TicketOverdues", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<EscalationReportViewModel> GetEscalationbyCategoryReport(string fromdate, string todate, string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<EscalationReportViewModel>("Usp_Report_Escalation", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DeletedTicketHistoryReportViewModel> GetDeletedTicketHistoryByCategoryReport(string fromdate, string todate, string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<DeletedTicketHistoryReportViewModel>("Usp_Report_DeletedTicketHistory", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PriorityWiseTicketStatusReportViewModel> GetPriorityWiseTicketStatusReport(string fromdate, string todate, string priorityId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@PriorityID", priorityId);
                    return con.Query<PriorityWiseTicketStatusReportViewModel>("Usp_Report_PriorityWiseTicketStatus", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserDetailsReportViewModel> GetUsersDetailsReport( string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserID", userId);
                    return con.Query<UserDetailsReportViewModel>("Usp_Report_UsersDetails", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserWiseCheckinCheckOutReportViewModel> UserWiseCheckinCheckOutReport(string fromdate, string todate, string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", fromdate);
                    param.Add("@ToDate", todate);
                    param.Add("@UserID", userId);
                    return con.Query<UserWiseCheckinCheckOutReportViewModel>("Usp_Report_CheckIn_CheckOut_Time_UserWise", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AdminYearExportReportViewModel> GetAgentCheckInOutMonthWiseReport(string year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@year", year);
                    return con.Query<AdminYearExportReportViewModel>("USP_AgentCheckInOutMonthWise", 
                        param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
