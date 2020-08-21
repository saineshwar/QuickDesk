using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class SlaPoliciesController : Controller
    {
        private readonly IPriority _priority;
        private readonly ISlaPolicies _slaPolicies;

        public SlaPoliciesController(IPriority priority, ISlaPolicies slaPolicies)
        {
            _priority = priority;
            _slaPolicies = slaPolicies;
        }

        [HttpGet]
        // GET: SLAPolicies
        public ActionResult Add()
        {
            SlaPoliciesViewModel slaPoliciesViewModel = new SlaPoliciesViewModel()
            {
                ListofPriority = _priority.GetAllPrioritySelectListItem()
            };

            return View(slaPoliciesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SlaPoliciesViewModel slaPoliciesViewModel)
        {
            if (ModelState.IsValid)
            {
                bool flag;
                int stage;
                StringBuilder message;
                ValidateSla(slaPoliciesViewModel, out flag, out stage, out message);
                if (flag == true)
                {
                    TempData["MessageErrorPolicies"] = message.ToString();
                }
                else
                {
                    if (_slaPolicies.CheckPoliciesExists(slaPoliciesViewModel.PriorityId))
                    {
                        ModelState.AddModelError("", "Priority already exists Delete Older One to Add New One");
                    }
                    else
                    {

                        

                        SlaPolicies slaPolicies = new SlaPolicies()
                        {
                            PriorityId = slaPoliciesViewModel.PriorityId,
                            CreateDate = DateTime.Now,
                            ResolutionResponseMins = slaPoliciesViewModel.ResolutionResponseMins,
                            FirstResponseDay = slaPoliciesViewModel.FirstResponseDay,
                            EveryResponseDay = slaPoliciesViewModel.EveryResponseDay,
                            ResolutionResponseHour = slaPoliciesViewModel.ResolutionResponseHour,
                            FirstResponseHour = slaPoliciesViewModel.FirstResponseHour,
                            EveryResponseMins = slaPoliciesViewModel.EveryResponseMins,
                            FirstResponseMins = slaPoliciesViewModel.FirstResponseMins,
                            ResolutionResponseDay = slaPoliciesViewModel.ResolutionResponseDay,
                            EveryResponseHour = slaPoliciesViewModel.EveryResponseHour,
                            EscalationDay = slaPoliciesViewModel.EscalationDay,
                            EscalationHour = slaPoliciesViewModel.EscalationHour,
                            EscalationMins = slaPoliciesViewModel.EscalationMins,
                            SlaPoliciesId = 0,
                            UserId = 0
                        };
                        var result = _slaPolicies.AddSlaPolicies(slaPolicies);

                        TempData["MessagePolicies"] = "Policy Added Successfully";

                    }
                }
            }

            slaPoliciesViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
            return View(slaPoliciesViewModel);
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult AllSlaPoliciesList(string priorityName, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var slaPoliciesCount = _slaPolicies.GetSlaPoliciesCount(priorityName);
                var listofPolicies = _slaPolicies.GetSlaPoliciesList(priorityName, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = listofPolicies, TotalRecordCount = slaPoliciesCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteSlaPolicies(int slaPoliciesId)
        {
            try
            {
                var result = _slaPolicies.DeleteSlaPolicies(slaPoliciesId);
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

        private void ValidateSla(SlaPoliciesViewModel slaPoliciesViewModel, out bool flag, out int stage, out StringBuilder message)
        {
            int counter = 0;
            message = new StringBuilder();
            bool localflag = false;
            // ReSharper disable once ReplaceWithSingleAssignment.False

            if (slaPoliciesViewModel.FirstResponseDay == null || slaPoliciesViewModel.FirstResponseHour == null || slaPoliciesViewModel.FirstResponseMins == null)
            {
                localflag = true;
                counter += 1;
                message.Append("First response time | ");
            }

            if (slaPoliciesViewModel.EveryResponseDay == null || slaPoliciesViewModel.EveryResponseHour == null || slaPoliciesViewModel.EveryResponseMins == null)
            {
                localflag = true;
                counter += 1;
                message.Append("Every response time  | ");
            }

            if (slaPoliciesViewModel.ResolutionResponseDay == null || slaPoliciesViewModel.ResolutionResponseHour == null || slaPoliciesViewModel.ResolutionResponseMins == null)
            {
                localflag = true;
                counter += 1;
                message.Append("Resolution time  | ");
            }

            if (slaPoliciesViewModel.EscalationDay == null || slaPoliciesViewModel.EscalationHour == null || slaPoliciesViewModel.EscalationMins == null)
            {
                localflag = true;
                counter += 1;
                message.Append("Escalation time  | ");
            }
            message.Append("Fields Cannot be Empty");
            stage = counter;
            flag = localflag;
        }

    }
}