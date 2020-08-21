using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]

    public class AssignmentloadController : Controller
    {
        private readonly IDefaultTicketSettings _iDefaultTicketSettings;

        public AssignmentloadController(IDefaultTicketSettings iDefaultTicketSettings)
        {
            _iDefaultTicketSettings = iDefaultTicketSettings;
        }

        // GET: Assignmentload
        public ActionResult Assign()
        {
            DefaultTicketSettingsViewModel defaultTicketSettingsViewModel = new DefaultTicketSettingsViewModel();
            var defaultModel = _iDefaultTicketSettings.GetDefaultTicketCount();
            if (defaultModel!= null)
            {
                defaultTicketSettingsViewModel.TicketsCount = defaultModel.TicketsCount;
                defaultTicketSettingsViewModel.AutoTicketsCloseHour = defaultModel.AutoTicketsCloseHour;
                defaultTicketSettingsViewModel.DefaultTicketId = defaultModel.DefaultTicketId;
            }
            return View(defaultTicketSettingsViewModel);
        }

        [HttpPost]
        public ActionResult Assign(DefaultTicketSettingsViewModel defaultTicketSettingsViewModel)
        {
            SessionHandler sessionHandler = new SessionHandler();
            if (ModelState.IsValid)
            {
              
                DefaultTicketSettings defaultTicketSettings = new DefaultTicketSettings();
                if (defaultTicketSettingsViewModel.DefaultTicketId == null)
                {
                    defaultTicketSettings.DefaultTicketId = 0;
                }
                else
                {
                    defaultTicketSettings.DefaultTicketId = defaultTicketSettingsViewModel.DefaultTicketId;
                }
                defaultTicketSettings.UserId = Convert.ToInt64(sessionHandler.UserId);
                defaultTicketSettings.TicketsCount = defaultTicketSettingsViewModel.TicketsCount;
                defaultTicketSettings.AutoTicketsCloseHour = defaultTicketSettingsViewModel.AutoTicketsCloseHour;
               
                _iDefaultTicketSettings.AddDefaultTicketCount(defaultTicketSettings);
            }

            TempData["TicketSettingsMessages"] = "DefaultTicket Saved Successfully";

            return RedirectToAction("Assign", "Assignmentload");
        }
    }
}