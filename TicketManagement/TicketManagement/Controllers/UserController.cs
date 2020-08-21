using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Common;
using TicketManagement.CommonData;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeUser]
    public class UserController : Controller
    {
        private readonly IProfile _iProfile;
        private readonly IUserMaster _userMaster;
        private readonly IPassword _password;
        readonly SessionHandler _sessionHandler = new SessionHandler();

        public UserController(IProfile profile, IUserMaster userMaster, IPassword password)
        {
            _iProfile = profile;
            _userMaster = userMaster;
            _password = password;
        }


        public ActionResult UserProfile()
        {
            try
            {
                var userprofile = _iProfile.GetprofileById(Convert.ToInt64(_sessionHandler.UserId));
                return View(userprofile);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UserProfile(UsermasterEditView usermasterEditView)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _iProfile.UpdateUserMasterDetails(Convert.ToInt64(_sessionHandler.UserId), usermasterEditView);

                if (result != 0)
                {
                    TempData["MessageProfileUpdate"] = CommonMessages.UserDetailsUpdateSuccessMessages;
                    return RedirectToAction("UserProfile");
                }
                else
                {
                    return View(usermasterEditView);
                }
            }
            else
            {
                return View(usermasterEditView);
            }

        }

        // GET: User
        public ActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changepassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var storedUserpassword = _password.GetPasswordbyUserId(Convert.ToInt64(_sessionHandler.UserId));
                    var usersalt = _userMaster.GetUserSaltbyUserid(Convert.ToInt64(_sessionHandler.UserId));
                    var generatehash = GenerateHashSha512.Sha512(changePasswordViewModel.ExistingPassword, usersalt.PasswordSalt);

                    if (changePasswordViewModel.ExistingPassword == changePasswordViewModel.NewPassword)
                    {
                        ModelState.AddModelError("", @"New Password Cannot be same as Old Password");
                        return View(changePasswordViewModel);
                    }

                    if (!string.Equals(storedUserpassword, generatehash, StringComparison.Ordinal))
                    {
                        ModelState.AddModelError("", "Current Password Entered is InValid");
                        return View(changePasswordViewModel);
                    }

                    if (!string.Equals(changePasswordViewModel.NewPassword, changePasswordViewModel.ConfirmPassword, StringComparison.Ordinal))
                    {
                        TempData["Reset_Error_Message"] = "Password Does not Match";
                        return View(changePasswordViewModel);
                    }
                    else
                    {

                        var salt = GenerateRandomNumbers.RandomNumbers(20);
                        var saltedpassword = GenerateHashSha512.Sha512(changePasswordViewModel.NewPassword, salt);
                        var result = _password.UpdatePasswordandHistory(Convert.ToInt64(_sessionHandler.UserId), saltedpassword, salt, "C");
                        TempData["MessageChangePassword"] = "Your Password Changed Successful";
                        return RedirectToAction("Changepassword", "User");
                    }
                }
                return View(changePasswordViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Profileimage(FormCollection frm)
        {

            try
            {
                foreach (string file in Request.Files)
                {
                    ProfileImage profileImage = new ProfileImage();
                    profileImage.ProfileImageId = 0;
                    string data = string.Empty;
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // and optionally write the file to disk
                        var fileName = Path.GetFileName(fileContent.FileName);
                        var fileextension = Path.GetExtension(fileContent.FileName);

                        if (fileName != null)
                        {
                            using (var fsp = fileContent.InputStream)
                            {
                                BinaryReader br = new System.IO.BinaryReader(fsp);
                                data = Convert.ToBase64String(br.ReadBytes(fileContent.ContentLength));
                            }
                        }
                    }

                    profileImage.ProfileImageBase64String = data;
                    profileImage.CreatedDate = DateTime.Now;
                    profileImage.UserId = Convert.ToInt64(_sessionHandler.UserId);
                    _iProfile.UpdateProfileImage(profileImage);

                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }

        [HttpPost]
        public ActionResult CheckIsProfileImageExists()
        {
            try
            {
                var result = _iProfile.IsProfileImageExists(Convert.ToInt64(_sessionHandler.UserId));
                if (result)
                {
                    string cacheProfileKey = Convert.ToString(_sessionHandler.CacheProfileKey);
                    string tempimageBase64String;
                    if (!CacheHelper.CheckExists(cacheProfileKey))
                    {
                        var imageBase64String =
                            _iProfile.GetProfileImageBase64String(Convert.ToInt64(_sessionHandler.UserId));
                        tempimageBase64String = string.Concat("data:image/png;base64,", imageBase64String);
                        CacheHelper.AddToCacheWithNoExpiration(cacheProfileKey, tempimageBase64String);
                    }
                    else
                    {
                        tempimageBase64String = (string)CacheHelper.GetStoreCachebyKey(cacheProfileKey);
                    }

                    return Json(new { result = true, base64string = tempimageBase64String },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, base64string = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteProfilepicture()
        {
            try
            {
                var isexists = _iProfile.IsProfileImageExists(Convert.ToInt64(_sessionHandler.UserId));
                if (isexists)
                {
                    var result = _iProfile.DeleteProfileImage(Convert.ToInt64(_sessionHandler.UserId));
                    if (result > 0)
                    {
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}