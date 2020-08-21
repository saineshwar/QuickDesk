using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeCommonFormUsersAttribute]
    public class ProfileController : Controller
    {
        private readonly IProfile _profile;
        public ProfileController(IProfile profile)
        {
            _profile = profile;
        }
        // GET: Profile
        public ActionResult Details(ProfileRequest profileRequest)
        {
            if (!string.IsNullOrEmpty(profileRequest.ProfileId))
            {
                var profile = _profile.GetUserprofileById(Convert.ToInt64(profileRequest.ProfileId));
                return PartialView("_Profile", profile);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult CheckIsProfileImageExists(ProfileRequest profileRequest)
        {
            try
            {
                var result = _profile.IsProfileImageExists(Convert.ToInt64(profileRequest.ProfileId));
                if (result)
                {
                    var imageBase64String =
                        _profile.GetProfileImageBase64String(Convert.ToInt64(profileRequest.ProfileId));
                    var tempimageBase64String = string.Concat("data:image/png;base64,", imageBase64String);
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
    }
}