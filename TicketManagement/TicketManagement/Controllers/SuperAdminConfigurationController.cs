using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Common.Emails;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class SuperAdminConfigurationController : Controller
    {
        private readonly IProcessSettings _processSettings;
        private readonly ISendingEmail _sendingEmail;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public SuperAdminConfigurationController(IProcessSettings processSettings, ISendingEmail sendingEmail)
        {
            _processSettings = processSettings;
            _sendingEmail = sendingEmail;
        }

        // GET: SuperAdminConfiguration
        [HttpGet]
        public ActionResult SmtpSettings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SmtpSettings(SmtpEmailSettingsViewModel smtpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var smtpEmailSettings = AutoMapper.Mapper.Map<SmtpEmailSettings>(smtpModel);
                    smtpEmailSettings.UserId = Convert.ToInt64(_sessionHandler.UserId);
                    smtpEmailSettings.CreatedDate = DateTime.Now;
                    smtpEmailSettings.SmtpProviderId = 0;
                    _processSettings.SaveSmtpSettings(smtpEmailSettings);
                    TempData["MessageSmtp"] = "SMTP Setting Save Successfully!";
                    return RedirectToAction("AllSmtpSettings", "SuperAdminConfiguration");
                }
                return View(smtpModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AllSmtpSettings()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllSmtpSettings(string search, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var totalCount = _processSettings.GetSmtpCount(search);
                var recordcount = _processSettings.GetAllTicketsbyUserId(search, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = recordcount, TotalRecordCount = totalCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditSmtpSettings(int? id)
        {
            if (id != null)
            {
                var smtpdata = _processSettings.EditSmtpSettings(id);
                return View(smtpdata);
            }

            return RedirectToAction("AllSmtpSettings", "SuperAdminConfiguration");
        }


        [HttpPost]
        public ActionResult EditSmtpSettings(SmtpEmailSettingsViewModel smtpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var smtpEmailSettings = AutoMapper.Mapper.Map<SmtpEmailSettings>(smtpModel);
                    _processSettings.UpdateSmtpSettings(smtpEmailSettings);
                    TempData["MessageSmtp"] = "SMTP Setting Save Successfully!";
                    return RedirectToAction("AllSmtpSettings", "SuperAdminConfiguration");
                }
                return View(smtpModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult SetDefaultConnection(RequestSmtp requestSmtp)
        {
            if (requestSmtp.SmtpProviderId != null)
            {
                var result = _processSettings.SettingDefaultConnection(requestSmtp.SmtpProviderId);

                if (result > 0)
                {
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TestConnection(RequestSmtp requestSmtp)
        {
            try
            {
                SendingEmailhelper testEmail = new SendingEmailhelper();
                var result = testEmail.TestEmail(Convert.ToString(Session["EmailId"]));
                if (result == "Successful")
                {
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult General()
        {
            try
            {
                var getsettings = _processSettings.GetGeneralSetting();
                return View(getsettings);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult General(GeneralSettingsViewModel generalSettingsViewModel)
        {
            try
            {
                var generalSettings = AutoMapper.Mapper.Map<GeneralSettings>(generalSettingsViewModel);
                _processSettings.InsertorUpdateGeneralSetting(generalSettings);
                TempData["MessageProfileUpdate"] = "Settings Save Successfully!";
                return RedirectToAction("General", "SuperAdminConfiguration");
            }
            catch (Exception)
            {
                throw;
            }
        }

       

    }
}