using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagement.CommonData;
using TicketManagement.Helpers;

namespace TicketManagement.Filters
{
    public class ValidateIsPasswordChangeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var changepasswordflag = Convert.ToString(HttpContext.Current.Session["ChangePasswordUserId"]);
                if (string.IsNullOrEmpty(changepasswordflag))
                {
                    filterContext.HttpContext.Session.Abandon();
                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                        { controller = "Error", action = "Error" }
                        ));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}