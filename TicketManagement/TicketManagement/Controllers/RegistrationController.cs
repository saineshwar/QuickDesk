using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using TicketManagement.Common;
using TicketManagement.Common.Emails;
using TicketManagement.CommonData;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;


namespace TicketManagement.Controllers
{
    [AllowAnonymous]
    public class RegistrationController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IVerify _verify;
        private readonly IVerification _verification;
        private readonly IAgentCheckInStatus _agentCheckInStatus;

        public RegistrationController(
            IUserMaster userMaster,
            IPassword password,
            ISavedAssignedRoles savedAssignedRoles,
            IVerify verify,
            IVerification verification,
            IAgentCheckInStatus agentCheckInStatus)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _verify = verify;
            _verification = verification;
            _agentCheckInStatus = agentCheckInStatus;
        }
        // GET: Registration

        // GET: Registration/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Registration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UsermasterView usermaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!this.IsCaptchaValid("Captcha is not valid"))
                    {
                        ModelState.AddModelError("", "Error: captcha is not valid.");
                        return View(usermaster);
                    }

                    if (_iUserMaster.CheckUsernameExists(usermaster.UserName))
                    {
                        ModelState.AddModelError("", "UserName already exists");
                        return View(usermaster);
                    }

                    if (_iUserMaster.CheckEmailIdExists(usermaster.EmailId))
                    {
                        ModelState.AddModelError("", "EmailId already exists");
                        return View(usermaster);
                    }

                    if (_iUserMaster.CheckMobileNoExists(usermaster.MobileNo))
                    {
                        TempData["MessageCreateUsersErrors"] = "MobileNo Already Exists";
                    }

                    var autoUsermaster = AutoMapper.Mapper.Map<Usermaster>(usermaster);
                    var salt = GenerateRandomNumbers.RandomNumbers(20);
                    var saltedpassword = GenerateHashSha512.Sha512(usermaster.Password, salt);

                    var userId = _iUserMaster.AddUser(autoUsermaster, saltedpassword, salt, Convert.ToInt16(StatusMain.Roles.User));
                    if (userId != -1)
                    {
                        var emailVerficationToken = GenerateHashSha256.ComputeSha256Hash((GenerateRandomNumbers.RandomNumbers(6)));
                        _verification.SendRegistrationVerificationToken(userId, emailVerficationToken);

                        SendingEmailhelper sendingEmailhelper = new SendingEmailhelper();
                        var name = string.Concat(usermaster.FirstName, usermaster.LastName);
                        await sendingEmailhelper.SendVerificationEmailasync(usermaster.EmailId, name, emailVerficationToken, "Registration", Convert.ToString(userId));

                        TempData["MessageRegistration"] = "Thank you. Your Registration has been completed successfully.";
                    }
                    else
                    {
                        TempData["ErrorRegistration"] = "Something Went Wrong While you are registering.Please try after sometime.";
                    }

                    return RedirectToAction("Register", "Registration");
                }
                else
                {
                    return View("Register", usermaster);
                }
            }
            catch
            {
                throw;
            }


        }

        public JsonResult CheckUsername(string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var result = _iUserMaster.CheckUsernameExists(username);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult CheckEmailIdExists(string emailid)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailid))
                {
                    var result = _verify.ValidateEmailIdExists(emailid);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
