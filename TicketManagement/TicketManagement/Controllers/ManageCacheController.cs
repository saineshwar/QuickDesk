using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Filters;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class ManageCacheController : Controller
    {
        // GET: ManageCache
        [HttpGet]
        public ActionResult Manage()
        {
            ViewBag.TotalCount = CacheHelper.TotalCachecount();
            return View();
        }


        [HttpPost]
        public ActionResult Manage(string submit)
        {
            if (submit == "RemoveallKeys")
            {
                CacheHelper.RemoveallKeys();
                MenuCacheHelper menuCacheHelper= new MenuCacheHelper();
                menuCacheHelper.LoadMenu();
            }

            TempData["MessageCache"] = "Cache Refreshed Successfully";
            ViewBag.TotalCount = CacheHelper.TotalCachecount();
            return View();
        }
    }
}