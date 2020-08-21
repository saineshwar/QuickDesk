using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TicketManagement.Filters
{
    public class CustomErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var result = new RedirectToRouteResult(new RouteValueDictionary(
                new { action = "Error", controller = "Error" }));

            filterContext.Result = result;
        }
    }
}