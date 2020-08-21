using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Common.Algorithm;
using TicketManagement.Helpers;

namespace TicketManagement.Filters
{
    public class HeaderAuthenticationAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    bool isAjax = filterContext.HttpContext.Request.IsAjaxRequest();

        //    if (isAjax)
        //    {
        //        // Grab the current request headers
        //        var headers = filterContext.HttpContext.Request.Headers;
        //        int expirationMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PkeyToken"]);

        //        // Ensure that all of your properties are present in the current Request
        //        if (!String.IsNullOrEmpty(headers["x-pkey-custom-header"]))
        //        {
        //            try
        //            {
        //                var token = headers["x-pkey-custom-header"];
        //                string computedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        //                AesAlgorithm aesAlgorithm = new AesAlgorithm();
        //                var key = aesAlgorithm.DecryptString(computedToken);


        //                string[] parts = key.Split(new char[] { ':' });
        //                var portalToken = parts[0];
        //                var userName = parts[1];
        //                var userId = parts[2];
        //                long ticks = long.Parse(parts[3]);
        //                DateTime timeStamp = new DateTime(ticks);
        //                bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > expirationMinutes;

        //                SessionHandler sessionHandler = new SessionHandler();

        //                if (sessionHandler.PortalToken != portalToken && sessionHandler.UserName != userName)
        //                {
        //                    filterContext.HttpContext.Session.Abandon();
        //                    filterContext.HttpContext.Session["ErrorMessage"] = "Access Denied";
        //                    filterContext.Result = new RedirectResult("/Error/Error");
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                filterContext.HttpContext.Session.Abandon();
        //                filterContext.HttpContext.Session["ErrorMessage"] = "Access Denied";
        //                filterContext.Result = new RedirectResult("/Error/Error");
        //            }
        //        }
        //        else
        //        {
        //            filterContext.HttpContext.Session.Abandon();
        //            filterContext.HttpContext.Session["ErrorMessage"] = "Access Denied";
        //            filterContext.Result = new RedirectResult("/Error/Error");
        //        }
        //    }
        //    else
        //    {
        //        filterContext.HttpContext.Session.Abandon();
        //        filterContext.HttpContext.Session["ErrorMessage"] = "Access Denied";
        //        filterContext.Result = new RedirectResult("/Error/Error");
        //    }

        //}
    }
}