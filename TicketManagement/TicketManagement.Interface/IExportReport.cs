using System.Collections.Generic;
using System.Web.Mvc;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IExportReport
    {
        List<SelectListItem> GetAllAgentListByCategoryId(string categoryId);

        List<SelectListItem> GetAllAgentandAgentAdminListByCategoryId(string categoryId);

        List<AgentAdminExportReportViewModel> GetDetailTicketStatusReport(string fromdate, string todate, string userId);

        List<CategoryExportReportViewModel> GetCategoryWiseTicketStatusReport(string fromdate, string todate, string categoryId);

        List<TicketOverduesViewModel> GetTicketOverduesbyCategoryReport(string fromdate, string todate, string overdueTypeId, string categoryId);

        List<EscalationReportViewModel> GetEscalationbyCategoryReport(string fromdate, string todate, string categoryId);

        List<DeletedTicketHistoryReportViewModel> GetDeletedTicketHistoryByCategoryReport(string fromdate, string todate, string categoryId);

        List<PriorityWiseTicketStatusReportViewModel> GetPriorityWiseTicketStatusReport(string fromdate, string todate,
            string priorityId);

        List<UserDetailsReportViewModel> GetUsersDetailsReport(string userId);

        List<UserWiseCheckinCheckOutReportViewModel> UserWiseCheckinCheckOutReport(string fromdate, string todate, string userId);

        List<AdminYearExportReportViewModel> GetAgentCheckInOutMonthWiseReport(string year);

    }
}