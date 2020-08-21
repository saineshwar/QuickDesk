using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class ExportReportViewModel
    {
        [Required(ErrorMessage = "Required From-date")]
        public string Fromdate { get; set; }

        [Required(ErrorMessage = "Required To-date")]
        public string Todate { get; set; }
        public List<SelectListItem> ListofReport { get; set; }
        public int ReportId { get; set; }
        public List<SelectListItem> ListStatus { get; set; }
        public int? StatusId { get; set; }
        public List<SelectListItem> ListCategory { get; set; }
        public int? CategoryId { get; set; }

    }

    public class ExportDateWiseTotalSummaryReport
    {
        public int Newtickets { get; set; }
        public string CreatedDate { get; set; }
        public int Opentickets { get; set; }
        public int Resolvedtickets { get; set; }
    }

    public class ExportDateWiseTotalDetailReport
    {
        public string TrackingId { get; set; }
        public string CreatedDate { get; set; }
        public string TicketUpdatedDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string OwnerName { get; set; }
    }
}
