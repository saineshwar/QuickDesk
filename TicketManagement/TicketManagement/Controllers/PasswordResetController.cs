using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using TicketManagement.Common.Emails;
using TicketManagement.Interface;
using TicketManagement.ViewModels;
using TicketManagement.CommonData;
using TicketManagement.Common;
using TicketManagement.Common.Algorithm;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using System.Configuration;

namespace TicketManagement.Controllers
{

    public class PasswordResetController : Controller
    {
        private readonly IVerify _verify;
        private readonly IVerification _verification;
        private readonly IPassword _iPassword;
        private readonly IUserMaster _userMaster;
        public PasswordResetController(IVerify verify, IPassword password, IUserMaster userMaster, IVerification verification)
        {
            _verify = verify;
            _iPassword = password;
            _userMaster = userMaster;
            _verification = verification;
        }

        [HttpGet]
        // GET: PasswordReset
        public ActionResult Process()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: PasswordReset
        public async Task<ActionResult> Process(ForgotPasswordModel resetPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _verify.ValidateEmailIdExists(resetPasswordModel.EmailId);

                    if (!result)
                    {
                        TempData["PasswordResetMessage"] = "In Valid EmailId";
                    }
                    else
                    {
                        SendingEmailhelper sendingEmailhelper = new SendingEmailhelper();
                        var generatecode = GenerateHashSha256.ComputeSha256Hash(GenerateRandomNumbers.RandomNumbers(6));
                        var userId = _userMaster.GetUserIdbyEmailId(Convert.ToString(resetPasswordModel.EmailId));
                        var count = _verification.GetSentResetPasswordVerificationCount(userId);
                        var sentEmailCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxResetEmailCount"]);
                        if (count == sentEmailCount)
                        {
                            TempData["PasswordResetMessage"] = "You Have exceed Limit for Resetting Password";
                        }
                        else
                        {
                            _verification.SendResetVerificationToken(userId, generatecode);
                            TempData["PasswordResetMessage"] = "Email Sent to your Account";
                            await sendingEmailhelper.SendForgotPasswordVerificationEmailasync(resetPasswordModel.EmailId, generatecode, "ForgotPassword", userId.ToString());
                        }
                      
                    }

                }
                return View(resetPasswordModel);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        [AuthorizeResetPassword]
        // GET: PasswordReset
        public ActionResult Reset()
        {
            return View(new ResetPasswordModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: PasswordReset
        public ActionResult Reset(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {

                var userId = Convert.ToInt64(Session["VerificationUserId"]);

                if (!string.Equals(resetPasswordModel.Password, resetPasswordModel.ConfirmPassword, StringComparison.Ordinal))
                {
                    TempData["Reset_Error_Message"] = "Password Does not Match";
                    return View(resetPasswordModel);
                }
                else
                {
                    var salt = GenerateRandomNumbers.RandomNumbers(20);
                    var saltedpassword = GenerateHashSha512.Sha512(resetPasswordModel.Password, salt);
                    var result = _iPassword.UpdatePasswordandHistory(userId, saltedpassword, salt, "R");

                    if (result)
                    {
                        var updateresult = _verification.UpdateResetVerification(userId);
                        Session["VerificationUserId"] = null;
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        TempData["Reset_Error_Message"] = "Something Went Wrong Please try again!";
                        return View(resetPasswordModel);
                    }
                }



            }

            return View(resetPasswordModel);
        }
    }
}