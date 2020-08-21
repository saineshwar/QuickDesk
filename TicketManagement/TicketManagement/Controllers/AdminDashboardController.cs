using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeAdmin]

    public class AdminDashboardController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly ITickets _iTickets;
        private readonly IPriority _priority;
        private readonly IStatus _status;
        private readonly IDashboardTicketCount _dashboardTicketCount;
        private readonly IChart _chart;
        private readonly IUserMaster _userMaster;
        private readonly IAllTicketGrid _allTicketGrid;
        private readonly ITicketHistory _ticketHistory;
        private readonly IMenuCategory _menuCategory;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        SessionHandler _sessionHandler = new SessionHandler();
        public AdminDashboardController(IMenu menu,
            ISubMenu subMenu,
            ITickets tickets,
            IPriority priority,
            IStatus status,
            IDashboardTicketCount dashboardTicketCount,
            IChart chart, IUserMaster userMaster, IAllTicketGrid allTicketGrid, ITicketHistory ticketHistory, IMenuCategory menuCategory)
        {
            _iMenu = menu;
            _iTickets = tickets;
            _priority = priority;
            _status = status;
            _dashboardTicketCount = dashboardTicketCount;
            _chart = chart;
            _userMaster = userMaster;
            _allTicketGrid = allTicketGrid;
            _ticketHistory = ticketHistory;
            _menuCategory = menuCategory;
        }

        // GET: AdminDashboard

        [HttpGet]
        public ActionResult Dashboard()
        {
            try
            {
                var ticketcounts = _dashboardTicketCount.GetAllTicketbyStatusofCurrentDayforAdmin();
                return View(ticketcounts);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ShowTickets(string statusId)
        {
            if (!string.IsNullOrEmpty(statusId))
            {
                if (statusId.Length == 1 || statusId.Length == 2)
                {
                    bool isIntString = statusId.All(char.IsDigit);
                    if (isIntString)
                    {
                        if (Enumerable.Range(1, 10).Contains(Convert.ToInt32(statusId)))
                        {
                            Session["GridStatusAdmin"] = statusId;
                        }
                        else
                        {
                            Session["GridStatusAdmin"] = "1";
                        }
                    }
                    else
                    {
                        Session["GridStatusAdmin"] = "1";
                    }
                }
                else
                {
                    Session["GridStatusAdmin"] = "1";
                }
            }
            return View();
        }

        public ActionResult ShowMenus()
        {
            try
            {
                var menuList = _menuCategory.ShowCategories(Convert.ToInt32(Session["RoleId"]));
                return PartialView("ShowMenuAdmin", menuList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllSearchFields()
        {
            try
            {
                var result = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "TrackingId",
                        Value = "1"
                    },
                    new SelectListItem()
                    {
                        Text = "Name",
                        Value= "2"
                    },
                    new SelectListItem()
                    {
                        Text = "Subject",
                        Value = "3"
                    }

                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetAllTickets(string search, string searchin, int? prioritysearch, int? statussearch, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                if (Session["GridStatusAdmin"] != null && statussearch == null)
                {
                    statussearch = Convert.ToInt32(Session["GridStatusAdmin"]);
                }

                var totalTicketsCount = _iTickets.GetAllTicketsCount(search, searchin, prioritysearch, statussearch);
                var recordcount = _iTickets.GetAllAdminTicketsbyList(search, searchin, prioritysearch, statussearch, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalTicketsCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllPriority()
        {
            try
            {
                var result = _priority.GetAllPrioritySelectListItem();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllStatus()
        {
            try
            {
                var result = _status.GetAllStatusSelectListItem();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ProcessChangeAllPriority(string[] ticketlist, short priority)
        {
            try
            {
                foreach (var ticketid in ticketlist)
                {
                    var changePriorityRequestModel = new ChangePriorityRequestModel
                    {
                        TicketId = Convert.ToInt64(ticketid),
                        PriorityId = priority
                    };
                    var result = _iTickets.ChangeTicketPriority(changePriorityRequestModel);
                }
                return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Fail" });
            }
        }

        public JsonResult ProcessChangeAllStatus(string[] ticketlist, short status)
        {
            try
            {
                foreach (var ticketid in ticketlist)
                {
                    var changeStatusRequestModel = new ChangeStatusRequestModel
                    {
                        TicketId = Convert.ToInt64(ticketid),
                        StatusId = status
                    };
                    var result = _iTickets.ChangeTicketStatus(changeStatusRequestModel);
                }
                return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Fail" });
            }
        }

        public JsonResult AssignTickettoUser(string[] ticketlist, long userId)
        {
            try
            {
                if (ticketlist.Length != 0)
                {
                    foreach (var ticketid in ticketlist)
                    {
                        var result = _iTickets.UpdateAssignTickettoUser(userId, Convert.ToInt64(ticketid));

                        TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                        TicketHistory ticketHistory = new TicketHistory
                        {
                            UserId = Convert.ToInt32(_sessionHandler.UserId),
                            Message = ticketHistoryHelper.AssignTickettoUser(),
                            ProcessDate = DateTime.Now,
                            TicketId = Convert.ToInt64(ticketid),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.ManuallyAssigedTicket)
                        };
                        _ticketHistory.TicketHistory(ticketHistory);
                    }
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "Fail" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Fail" });
            }
        }

        public ActionResult Keepalive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowBarCharts()
        {
            try
            {
                var listofdata = _chart.GetListofBarChartDataAdmin();
                return Json(listofdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ShowDonutCharts()
        {
            List<DonutChartResponse> listoChartResponses = new List<DonutChartResponse>();

            var dictionary = new List<DonutChartRequest>();
            try
            {
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Open",
                    value = 1
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Resolved",
                    value = 2
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "InProgress",
                    value = 3
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "OnHold",
                    value = 4
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "RecentlyEdited",
                    value = 5
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Replied",
                    value = 6
                }
                );
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Deleted",
                    value = 7
                });
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Overdue",
                    value = 8
                });
                dictionary.Add(new DonutChartRequest()
                {
                    label = "Escalation",
                    value = 9
                });

                var listofdata = _chart.GetListofDonutChartDataAdmin();

                for (int i = 0; i < listofdata.Count; i++)
                {
                    var tempstatusid = listofdata[i].StatusId;

                    var getdata = (from a in dictionary
                                   where a.value == tempstatusid
                                   select a.label).FirstOrDefault();

                    if (tempstatusid == 1)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].New
                        });
                    }
                    else if (tempstatusid == 2)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].Resolved
                        });
                    }
                    else if (tempstatusid == 3)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].InProgress
                        });
                    }
                    else if (tempstatusid == 4)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].OnHold
                        });
                    }
                    else if (tempstatusid == 5)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].RecentlyEdited
                        });
                    }
                    else if (tempstatusid == 6)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].Replied
                        });
                    }
                    else if (tempstatusid == 7)
                    {
                        listoChartResponses.Add(new DonutChartResponse()
                        {
                            label = getdata,
                            value = listofdata[i].Deleted
                        });
                    }
                }

                return Json(listoChartResponses, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DeletedTickets()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllDeletedTickets(string search, string searchin, int? prioritysearch, int? statussearch, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var currentUserId = Convert.ToInt64(_sessionHandler.UserId);
                var totalTicketsCount = _iTickets.GetDeleteTicketsAgentCount(currentUserId);
                var recordcount = _iTickets.GetAllDeleteTickets(search, searchin, prioritysearch, statussearch, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalTicketsCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult RestoreTickets(string[] ticketlist)
        {
            try
            {
                if (ticketlist.Length != 0)
                {
                    foreach (var ticketid in ticketlist)
                    {
                        var result = _iTickets.RestoreTicket(Convert.ToInt64(_sessionHandler.UserId), Convert.ToInt64(ticketid));

                        TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                        TicketHistory ticketHistory = new TicketHistory
                        {
                            UserId = Convert.ToInt32(_sessionHandler.UserId),
                            Message = ticketHistoryHelper.TicketRestore(),
                            ProcessDate = DateTime.Now,
                            TicketId = Convert.ToInt64(ticketid),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.Restored)
                        };
                        _ticketHistory.TicketHistory(ticketHistory);

                       
                    }
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "Fail" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Fail" });
            }
        }

        public ActionResult GetAllAgentUsers(string usernames)
        {
            var userlist = _userMaster.GetAutoCompleteUserName(usernames,Convert.ToInt32(StatusMain.Roles.Admin));
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTicket(string ticketid)
        {
            try
            {
                if (!string.IsNullOrEmpty(ticketid))
                {
                    var result = _iTickets.DeleteTicket(Convert.ToInt64(_sessionHandler.UserId), Convert.ToInt64(ticketid), Convert.ToInt16(StatusMain.Status.Deleted));

                    if (result > 0)
                    {
                        TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                        TicketHistory ticketHistory = new TicketHistory
                        {
                            UserId = Convert.ToInt32(_sessionHandler.UserId),
                            Message = ticketHistoryHelper.TicketDelete(),
                            ProcessDate = DateTime.Now,
                            TicketId = Convert.ToInt64(ticketid),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeleteTicket)
                        };
                        _ticketHistory.TicketHistory(ticketHistory);

                    }
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = string.Empty });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult ShowAllTickets()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllAdminTickets(string search, string searchin, int? prioritysearch, int? statussearch, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var totalTicketsCount = _allTicketGrid.AllAdminTicketsCount(search, searchin, prioritysearch, statussearch);
                var recordcount = _allTicketGrid.AllAdminTickets(search, searchin, prioritysearch, statussearch, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalTicketsCount });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}