using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagement.Filters
{
    public class AuthTokenChecker : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string sessionAuthValue = Convert.ToString(filterContext.HttpContext.Session["AuthenticationToken"]);
            var cookieAuthValue = filterContext.HttpContext.Request.Cookies["AuthenticationToken"];

            if (string.IsNullOrEmpty(sessionAuthValue))
            {
                filterContext.HttpContext.Session.Abandon();
                filterContext.Result = new RedirectResult("/Error/Error");
            }

            if (cookieAuthValue == null)
            {
                filterContext.HttpContext.Session.Abandon();
                filterContext.Result = new RedirectResult("/Error/Error");
            }

            if (!string.IsNullOrEmpty(sessionAuthValue) && cookieAuthValue != null)
            {
                string cookieAuthTokenValue = Convert.ToString(filterContext.HttpContext.Request.Cookies["AuthenticationToken"].Value);

                if (!string.Equals(sessionAuthValue, cookieAuthTokenValue))
                {
                    filterContext.HttpContext.Session.Abandon();
                    filterContext.Result = new RedirectResult("/Error/Error");
                }
            }

        }
    }
}