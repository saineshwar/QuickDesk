using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Helpers;


namespace TicketManagement.Filters
{
    public class SessionCheckAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionHandler sessionHandler = new SessionHandler();
            if (string.IsNullOrEmpty(Convert.ToString(sessionHandler.UserId)))
            {
                filterContext.HttpContext.Session.Clear();
                filterContext.HttpContext.Session.Abandon();
                filterContext.Result = new RedirectResult("/login/login");
            }
        }
    }

}