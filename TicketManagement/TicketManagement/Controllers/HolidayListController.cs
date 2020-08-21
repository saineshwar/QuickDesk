using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class HolidayListController : Controller
    {
        private readonly IHolidayList _holidayList;
        public HolidayListController(IHolidayList holidayList)
        {
            _holidayList = holidayList;
        }

        // GET: HolidayList
        [HttpGet]
        public ActionResult Add()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Add(HolidayViewModel holidayViewModel)
        {
            if (ModelState.IsValid)
            {
                _holidayList.AddHoliday(new HolidayList()
                {
                    HolidayName = holidayViewModel.HolidayName,
                    HolidayId = 0,
                    HolidayDate = Convert.ToDateTime(holidayViewModel.HolidayDate)
                });
            }

            return RedirectToAction("Add", "HolidayList");
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult AllHolidayList(string holidayName,  int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var holidayCount = _holidayList.GetHolidayCount(holidayName);
                var listofholiday = _holidayList.GetHolidayList(holidayName, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = listofholiday, TotalRecordCount = holidayCount });
            }
            catch (Exception)
            {
                throw;
            }
        }


        public JsonResult Delete(int? holidayId)
        {
            try
            {
                if (holidayId == null)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                _holidayList.DeleteHoliday(holidayId);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}