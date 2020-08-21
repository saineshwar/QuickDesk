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
    public class VerifyRegistrationController : Controller
    {
        private readonly IVerification _verificationRepository;
        public VerifyRegistrationController(IVerification verificationRepository)
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
                        var rvModel = _verificationRepository.GetRegistrationGeneratedToken(arrayVakue[1]);
                        if (rvModel != null)
                        {
                            var result = TicketSecurityManager.IsTokenValid(arrayVakue, hashtoken, rvModel.GeneratedToken);

                            if (_verificationRepository.CheckIsAlreadyVerifiedRegistration(Convert.ToInt64(arrayVakue[1])))
                            {
                                TempData["TokenErrorMessage"] = "Sorry Link is Expired";
                                return RedirectToAction("Login", "Login");
                            }

                            if (result == 1)
                            {
                                TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Login");
                            }

                            if (result == 2)
                            {
                                TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Login");
                            }

                            if (result == 0)
                            {
                                
                                Session["VerificationUserId"] = arrayVakue[1];
                                var updateresult = _verificationRepository.UpdateRegisterVerification(Convert.ToInt64(arrayVakue[1]));
                                if (updateresult)
                                {
                                    TempData["Verify"] = "Done";
                                    return RedirectToAction("Completed", "VerifyRegistration");
                                }
                                else
                                {
                                    TempData["TokenErrorMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                                    return RedirectToAction("Login", "Login");
                                }

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


        [HttpGet]
        public ActionResult Completed()
        {
            if (Convert.ToString(TempData["Verify"]) == "Done")
            {
                TempData["RegistrationCompleted"] = "Registration Process Completed. Now you can Login and Access Account.";
                return View();
            }
            else
            {
                TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                return RedirectToAction("Login", "Login");
            }

        }

    }
}