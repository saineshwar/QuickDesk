using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.ViewModels;


namespace TicketManagement.Controllers
{
    [AuthorizeUser]
    public class UserDashboardController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly ITickets _iTickets;
        private readonly IDashboardTicketCount _dashboardTicketCount;
        private readonly IAllTicketGrid _allTicketGrid;
        private readonly IMenuCategory _menuCategory;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public UserDashboardController(IMenu menu, ITickets tickets, IDashboardTicketCount dashboardTicketCount, IAllTicketGrid allTicketGrid, IMenuCategory menuCategory)
        {
            _iMenu = menu;
            _iTickets = tickets;
            _dashboardTicketCount = dashboardTicketCount;
            _allTicketGrid = allTicketGrid;
            _menuCategory = menuCategory;
        }

        // GET: UserDashboard
        public ActionResult Dashboard()
        {
            try
            {
                var ticketcounts = _dashboardTicketCount.GetAllTicketbyStatusofCurrentDayforUser(Convert.ToInt64(_sessionHandler.UserId));
                return View(ticketcounts);
            }
            catch (Exception)
            {

                throw;
            }
        }

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
                            Session["GridStatusUser"] = statusId;
                        }
                        else
                        {
                            Session["GridStatusUser"] = "1";
                        }
                    }
                    else
                    {
                        Session["GridStatusUser"] = "1";
                    }
                }
                else
                {
                    Session["GridStatusUser"] = "1";
                }
            }
            return View();
        }

        public ActionResult ShowMenus()
        {
            try
            {
                var menuList = _menuCategory.ShowCategories(Convert.ToInt32(_sessionHandler.RoleId));
                return PartialView("ShowMenuUser", menuList);
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
        public JsonResult GetAllTickets(string search, string searchin, int? statusId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {

                if (Session["GridStatusUser"] != null && statusId == null)
                {
                    statusId = Convert.ToInt32(Session["GridStatusUser"]);
                }

                var currentUserId = Convert.ToInt64(_sessionHandler.UserId);
                var totalTicketsCount = _iTickets.GetTicketsCount(currentUserId, search, statusId, searchin);
                var recordcount = _iTickets.GetAllTicketsbyUserId(search, searchin, statusId, currentUserId, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalTicketsCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ShowAllTickets()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllUserTickets(string search, string searchin, int? statusId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var currentUserId = Convert.ToInt64(_sessionHandler.UserId);
                var totalTicketsCount = _allTicketGrid.AllUserTicketsCount(currentUserId, search, statusId, searchin);
                var recordcount = _allTicketGrid.AllUserTickets(search, searchin, statusId, currentUserId, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalTicketsCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}