using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeAgentsAdmin]
    public class AgentAdminReportController : Controller
    {
        private readonly IExportReport _exportReport;
        private readonly IOverdueTypes _overdueTypes;
        private readonly IPriority _priority;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public AgentAdminReportController(IExportReport agentAdminReport, IOverdueTypes overdueTypes, IPriority priority)
        {
            _exportReport = agentAdminReport;
            _overdueTypes = overdueTypes;
            _priority = priority;
        }
        // GET: AgentAdminReport
        public ActionResult Report()
        {
            var agentAdminReportViewModel = new AgentAdminReportViewModel
            {
                ListofAgent = _exportReport.GetAllAgentListByCategoryId(_sessionHandler.AgentAdminCategoryId),
                ListofReport = ReportList(),
                ListofOverdueTypes = _overdueTypes.GetAllActiveOverdueTypes(),
                ListofPriority = _priority.GetAllPrioritySelectListItem()
            };
            return View(agentAdminReportViewModel);
        }

        [HttpPost]
        public ActionResult Report(AgentAdminReportViewModel reportViewModel)
        {
            if (ModelState.IsValid)
            {
                if (reportViewModel.ReportId == 1)
                {
                    var reportDetailTicketStatusReport = _exportReport.GetDetailTicketStatusReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.AgentId);

                    if (reportDetailTicketStatusReport != null && reportDetailTicketStatusReport.Count > 0)
                    {
                        ExporttoExcel(reportDetailTicketStatusReport, "Report", "AgentDetailTicketStatusReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 2)
                {
                    var reportCategoryWiseTicketStatuReport = _exportReport.GetCategoryWiseTicketStatusReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, _sessionHandler.AgentAdminCategoryId);

                    if (reportCategoryWiseTicketStatuReport != null && reportCategoryWiseTicketStatuReport.Count > 0)
                    {
                        ExporttoExcel(reportCategoryWiseTicketStatuReport, "Report", "CategoryWiseTicketStatusReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 3)
                {
                    var reportTicketOverduesbyCategorReport = _exportReport.GetTicketOverduesbyCategoryReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.OverdueTypeId, _sessionHandler.AgentAdminCategoryId);

                    if (reportTicketOverduesbyCategorReport != null && reportTicketOverduesbyCategorReport.Count > 0)
                    {
                        ExporttoExcel(reportTicketOverduesbyCategorReport, "Report", "TicketOverduesReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 4)
                {
                    var reportTicketOverdueReport = _exportReport.GetTicketOverduesbyCategoryReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.OverdueTypeId, _sessionHandler.AgentAdminCategoryId);

                    if (reportTicketOverdueReport != null && reportTicketOverdueReport.Count > 0)
                    {
                        ExporttoExcel(reportTicketOverdueReport, "Report", "TicketOverduesReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 5)
                {
                    var reportEscalationReport = _exportReport.GetEscalationbyCategoryReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, _sessionHandler.AgentAdminCategoryId);

                    if (reportEscalationReport != null && reportEscalationReport.Count > 0)
                    {
                        ExporttoExcel(reportEscalationReport, "Report", "EscalationbyCategoryReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 6)
                {
                    var reportDeletedReport = _exportReport.GetDeletedTicketHistoryByCategoryReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, _sessionHandler.AgentAdminCategoryId);

                    if (reportDeletedReport != null && reportDeletedReport.Count > 0)
                    {
                        ExporttoExcel(reportDeletedReport, "Report", "DeletedTicketHistoryByCategoryRepor.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 7)
                {
                    var reportPriorityReport = _exportReport.GetPriorityWiseTicketStatusReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.PriorityId);

                    if (reportPriorityReport != null && reportPriorityReport.Count > 0)
                    {
                        ExporttoExcel(reportPriorityReport, "Report", "PriorityWiseTicketStatusReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

                if (reportViewModel.ReportId == 8)
                {
                    var reportUserReport = _exportReport.GetUsersDetailsReport(
                        reportViewModel.AgentId
                      );

                    if (reportUserReport != null && reportUserReport.Count > 0)
                    {
                        ExporttoExcel(reportUserReport, "Report", "UsersDetailsReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }
                if (reportViewModel.ReportId == 9)
                {
                    var checkinCheckOutReport = _exportReport.UserWiseCheckinCheckOutReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.AgentId
                    );

                    if (checkinCheckOutReport != null && checkinCheckOutReport.Count > 0)
                    {
                        ExporttoExcel(checkinCheckOutReport, "Report", "UserWiseCheckinCheckOutReport.xlsx");
                    }
                    else
                    {
                        TempData["ReportMessages"] = "No Data to Export";
                    }
                }

            }

            reportViewModel.ListofAgent = _exportReport.GetAllAgentListByCategoryId(_sessionHandler.AgentAdminCategoryId);
            reportViewModel.ListofReport = ReportList();
            reportViewModel.ListofOverdueTypes = _overdueTypes.GetAllActiveOverdueTypes();
            reportViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
            return View(reportViewModel);
        }

        private void ExporttoExcel<T>(List<T> table, string filename, string reportname)
        {
            string tempfilename = $"filename={reportname}";
            HttpContext.Response.Clear();
            HttpContext.Response.ClearContent();
            HttpContext.Response.ClearHeaders();
            HttpContext.Response.Buffer = true;
            HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Response.AddHeader(@"content-disposition", "attachment;" + tempfilename);

            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
                ws.Cells["A1"].LoadFromCollection(table, true);
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(HttpContext.Response.OutputStream);
            }

            HttpContext.Response.Flush();
            HttpContext.Response.End();
        }

        public List<SelectListItem> ReportList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "AgentWise Ticket Status Report",Value = "1",
                },
                new SelectListItem()
                {
                    Text = "CategoryWise Ticket Status Report",Value = "2",
                },
                new SelectListItem()
                {
                    Text = "Ticket Overdues Status Report",Value = "3",
                },
                new SelectListItem()
                {
                    Text = "Ticket Overdues UserWise Report",Value = "4",
                },
                new SelectListItem()
                {
                    Text = "Ticket Escalation Report",Value = "5",
                },
                new SelectListItem()
                {
                    Text = "Ticket Deleted Report",Value = "6",
                },
                new SelectListItem()
                {
                    Text = "PriorityWise Ticket Status Report",Value = "7",
                },
                new SelectListItem()
                {
                    Text = "Agent Detail Report",Value = "8",
                },
                new SelectListItem()
                {
                    Text = "UserWise Checkin CheckOut Report",Value = "9",
                },
            };
            return listofReport;
        }
    }
}