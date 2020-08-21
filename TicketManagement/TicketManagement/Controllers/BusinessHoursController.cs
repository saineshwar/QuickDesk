using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class BusinessHoursController : Controller
    {
        private readonly IBusinessHours _businessHours;
        public BusinessHoursController(IBusinessHours businessHours)
        {
            _businessHours = businessHours;
        }
        // GET: BusinessHours
        public ActionResult Add()
        {
            BusinessHoursModel businessHoursModel = new BusinessHoursModel()
            {
                ListofDays = Dayslist(),
                ListofHour = Hourlist(),
                ListofPeriod = Periodslist(),
                SelectedDays = new List<string>(),
                ListofBusinessHoursType = _businessHours.ListofBusinessHoursType()
            };
            return View(businessHoursModel);
        }

        [HttpPost]
        public ActionResult Add(
            BusinessHoursModel model,
            List<string> morningHour,
            List<string> morningPeriod,
            List<string> eveningHour,
            List<string> eveningPeriod)
        {

            if (model.SelectedBusinessHoursType == "1")
            {
                BusinessHours businessHours = new BusinessHours()
                {
                    BusinessHoursId = 0,
                    Description = model.Description,
                    HelpdeskHoursType = Convert.ToInt32(model.SelectedBusinessHoursType),
                    Name = model.Name
                };

                _businessHours.AddBusinessHours(businessHours);

                TempData["BusinessHoursMessage"] = "BusinessHours Saved Successfully";
            }
            else
            {
                if (model.SelectedDays == null)
                {
                    ModelState.AddModelError("", "Please SelectedDays");
                }
                else
                {
                    BusinessHours businessHours = new BusinessHours
                    {
                        BusinessHoursId = 0,
                        Description = model.Description,
                        HelpdeskHoursType = Convert.ToInt32(model.SelectedBusinessHoursType),
                        Name = model.Name
                    };

                    List<BusinessHoursDetails> listBusinessHoursDetails = new List<BusinessHoursDetails>();

                    for (int i = 0; i < model.SelectedDays.Count(); i++)
                    {
                        var currentMorningHour = morningHour[i];
                        var currentMorningPeriod = morningPeriod[i];
                        var currentEveningHour = eveningHour[i];
                        var currentEveningPeriod = eveningPeriod[i];
                        var currentday = model.SelectedDays[i];

                        var starthours = DateTime.Parse($"{currentMorningHour + ":00" + " " + currentMorningPeriod.ToUpper()}").ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                        var endhours = DateTime.Parse($"{currentEveningHour + ":00" + " " + currentEveningPeriod.ToUpper()}").ToString("HH:mm:ss", CultureInfo.InvariantCulture);


                        BusinessHoursDetails businessHoursDetails = new BusinessHoursDetails()
                        {
                            BusinessHoursId = 0,
                            BusinessHoursDetailsId = 0,
                            CreateDate = DateTime.Now,
                            Day = currentday,
                            MorningTime = $"{currentMorningHour + ":00" + " " + currentMorningPeriod.ToUpper()}",
                            EveningTime = $"{currentEveningHour + ":00" + " " + currentEveningPeriod.ToUpper()}",
                            MorningPeriods = currentMorningPeriod,
                            EveningPeriods = currentEveningPeriod,
                            StartTime = starthours,
                            CloseTime = endhours
                        };

                        listBusinessHoursDetails.Add(businessHoursDetails);
                    }

                    _businessHours.AddBusinessHours(businessHours, listBusinessHoursDetails);
                    TempData["BusinessHoursMessage"] = "BusinessHours Saved Successfully";

                    return RedirectToAction("Add", "BusinessHours");
                }
            }

            model.ListofDays = Dayslist();
            model.ListofHour = Hourlist();
            model.ListofPeriod = Periodslist();
            model.SelectedDays = new List<string>();
            model.ListofBusinessHoursType = _businessHours.ListofBusinessHoursType();
            return View(model);
        }

        private List<SelectListItem> Hourlist()
        {
            List<SelectListItem> hourlist = new List<SelectListItem>();
            hourlist.Add(new SelectListItem() { Text = "1:00", Value = "1:00" });
            hourlist.Add(new SelectListItem() { Text = "1:30", Value = "1:30" });
            hourlist.Add(new SelectListItem() { Text = "2:00", Value = "2:00" });
            hourlist.Add(new SelectListItem() { Text = "2:30", Value = "2:30" });
            hourlist.Add(new SelectListItem() { Text = "3:00", Value = "3:00" });
            hourlist.Add(new SelectListItem() { Text = "3:30", Value = "3:30" });
            hourlist.Add(new SelectListItem() { Text = "4:00", Value = "4:00" });
            hourlist.Add(new SelectListItem() { Text = "4:30", Value = "4:30" });
            hourlist.Add(new SelectListItem() { Text = "5:00", Value = "5:00" });
            hourlist.Add(new SelectListItem() { Text = "5:30", Value = "5:30" });
            hourlist.Add(new SelectListItem() { Text = "6:00", Value = "6:00" });
            hourlist.Add(new SelectListItem() { Text = "6:30", Value = "6:30" });
            hourlist.Add(new SelectListItem() { Text = "7:00", Value = "7:00" });
            hourlist.Add(new SelectListItem() { Text = "7:30", Value = "7:30" });
            hourlist.Add(new SelectListItem() { Text = "8:00", Value = "8:00" });
            hourlist.Add(new SelectListItem() { Text = "8:30", Value = "8:30" });
            hourlist.Add(new SelectListItem() { Text = "9:00", Value = "9:00" });
            hourlist.Add(new SelectListItem() { Text = "9:30", Value = "9:30" });
            hourlist.Add(new SelectListItem() { Text = "10:00", Value = "10:00" });
            hourlist.Add(new SelectListItem() { Text = "10:30", Value = "10:30" });
            hourlist.Add(new SelectListItem() { Text = "11:00", Value = "11:00" });
            hourlist.Add(new SelectListItem() { Text = "11:30", Value = "11:30" });
            hourlist.Add(new SelectListItem() { Text = "12:00", Value = "12:00" });
            hourlist.Add(new SelectListItem() { Text = "12:30", Value = "12:30" });
            return hourlist;
        }

        private List<SelectListItem> Periodslist()
        {
            List<SelectListItem> periodslist = new List<SelectListItem>();
            periodslist.Add(new SelectListItem() { Text = "am", Value = "am" });
            periodslist.Add(new SelectListItem() { Text = "pm", Value = "pm" });
            return periodslist;
        }

        private List<SelectListItem> Dayslist()
        {
            List<SelectListItem> dayslist = new List<SelectListItem>();
            dayslist.Add(new SelectListItem() { Text = "Monday", Value = "Monday" });
            dayslist.Add(new SelectListItem() { Text = "Tuesday", Value = "Tuesday" });
            dayslist.Add(new SelectListItem() { Text = "Wednesday", Value = "Wednesday" });
            dayslist.Add(new SelectListItem() { Text = "Thursday", Value = "Thursday" });
            dayslist.Add(new SelectListItem() { Text = "Friday", Value = "Friday" });
            dayslist.Add(new SelectListItem() { Text = "Saturday", Value = "Saturday" });
            dayslist.Add(new SelectListItem() { Text = "Sunday", Value = "Sunday" });
            return dayslist;
        }


        public ActionResult AllBusinessHours()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult AllBusinessHoursList(string name, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var businessHoursCount = _businessHours.GetBusinessHoursCount(name);
                var listofbusinessHours = _businessHours.GetBusinessList(name, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = listofbusinessHours, TotalRecordCount = businessHoursCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteBusinessHours(int businessHoursId)
        {
            try
            {
                var result = _businessHours.DeleteBusinessHours(businessHoursId);
                if (result == 0)
                {
                    return Json(new { Result = "NO" }, JsonRequestBehavior.AllowGet);
                }
                else if (result == -1)
                {
                    return Json(new { Result = "ERROR", Message = "Cannot Delete" });
                }

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Cannot Delete" });
            }
        }

        public ActionResult Details(int? id)
        {
            var detailsBusinessHours = _businessHours.DetailsBusinessHours(id);
            return View(detailsBusinessHours);
        }
    }
}