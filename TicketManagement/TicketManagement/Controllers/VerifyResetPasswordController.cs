using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Helpers;
using TicketManagement.Interface;

namespace TicketManagement.Controllers
{
    public class VerifyResetPasswordController : Controller
    {
        private readonly IVerification _verificationRepository;
        public VerifyResetPasswordController(IVerification verificationRepository)
        {
            _verificationRepository = verificationRepository;
        }


        [HttpGet]
        public ActionResult Verify(string key, string hashtoken)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(hashtoken))
                {
                    var arrayVakue = TicketSecurityManager.SplitToken(key);
                    if (arrayVakue != null)
                    {
                        // arrayVakue[1] "UserId"
                        var rvModel = _verificationRepository.GetResetGeneratedToken(arrayVakue[1]);
                        if (rvModel != null)
                        {
                            var result = TicketSecurityManager.IsTokenValid(arrayVakue, hashtoken, rvModel.GeneratedToken);

                            if (result == 1)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Login");
                            }

                            if (result == 2)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Login");
                            }

                            if (result == 0)
                            {
                                Session["VerificationUserId"] = arrayVakue[1];
                                Session["ActiveVerification"] = "1";

                                return RedirectToAction("Reset", "PasswordReset");
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                return RedirectToAction("Login", "Login");
            }

            TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
            return RedirectToAction("Login", "Login");
        }
    }
}