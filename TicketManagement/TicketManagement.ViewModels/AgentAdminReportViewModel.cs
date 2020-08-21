using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class AgentAdminReportViewModel
    {
        public string AgentId { get; set; }
        public List<SelectListItem> ListofAgent { get; set; }

        public string Fromdate { get; set; }

        public string Todate { get; set; }

        public List<SelectListItem> ListofReport { get; set; }
        public int ReportId { get; set; }

        public string OverdueTypeId { get; set; }
        public List<SelectListItem> ListofOverdueTypes { get; set; }

        public string PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }
    }

    public class HodReportViewModel
    {
        public string AgentId { get; set; }
        public List<SelectListItem> ListofAgent { get; set; }

        public string Fromdate { get; set; }

        public string Todate { get; set; }

        public List<SelectListItem> ListofReport { get; set; }
        public int ReportId { get; set; }

        public string OverdueTypeId { get; set; }
        public List<SelectListItem> ListofOverdueTypes { get; set; }

        public string PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }
    }

    public class AgentAdminExportReportViewModel
    {
        public string Name { get; set; }
        public string TicketAssignedDate { get; set; }
        public int? TotalTickets { get; set; }
        public int? OpenTicket { get; set; }
        public int? ResolvedTicket { get; set; }
        public int? InProgressTicket { get; set; }
        public int? OnHoldTicket { get; set; }
        public int? RecentlyEditedTicket { get; set; }
        public int? RepliedTicket { get; set; }
        public int? DeletedTicket { get; set; }
        public int? OverdueTicket { get; set; }
        public int? EscalationTicket { get; set; }
        public int? ClosedTicket { get; set; }

    }

    public class AdminExportReportViewModel
    {
        public string Name { get; set; }
        public string TicketAssignedDate { get; set; }
        public int? TotalTickets { get; set; }
        public int? OpenTicket { get; set; }
        public int? ResolvedTicket { get; set; }
        public int? InProgressTicket { get; set; }
        public int? OnHoldTicket { get; set; }
        public int? RecentlyEditedTicket { get; set; }
        public int? RepliedTicket { get; set; }
        public int? DeletedTicket { get; set; }
        public int? OverdueTicket { get; set; }
        public int? EscalationTicket { get; set; }
        public int? ClosedTicket { get; set; }

    }

    public class AdminReportViewModel
    {
        public string CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        public string AgentId { get; set; }
        public List<SelectListItem> ListofAgent { get; set; }

        public string Fromdate { get; set; }

        public string Todate { get; set; }

        public List<SelectListItem> ListofReport { get; set; }
        public int ReportId { get; set; }

        public string OverdueTypeId { get; set; }
        public List<SelectListItem> ListofOverdueTypes { get; set; }

        public string PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }
    }

    public class AdminYearReportViewModel
    {
        [Required(ErrorMessage = "Please select Year")]
        public string Year { get; set; }
        public List<SelectListItem> ListofYears { get; set; }
    }

    public class AdminYearExportReportViewModel
    {
        public string Users { get; set; }
        public string ReportYear { get; set; }
        public string JAN { get; set; }
        public string FEB { get; set; }
        public string MAR { get; set; }
        public string APR { get; set; }
        public string MAY { get; set; }
        public string JUNE { get; set; }
        public string JULY { get; set; }
        public string AUG { get; set; }
        public string SEP { get; set; }
        public string OCT { get; set; }
        public string NOV { get; set; }
        public string DEC { get; set; }
    }
}
