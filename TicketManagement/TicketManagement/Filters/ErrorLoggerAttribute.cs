using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Helpers;

namespace TicketManagement.Filters
{
    public class ErrorLoggerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        filterContext.Exception.Message,
                        filterContext.Exception.StackTrace
                    }
                };
                if (filterContext.ExceptionHandled == false)
                {
                    ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
                }

                filterContext.ExceptionHandled = true;
            }
            else
            {
                if (filterContext.ExceptionHandled == false)
                {
                    ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
                }

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Session.Abandon();
                filterContext.Result = new RedirectResult("/Error/Error");
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
                filterContext.RequestContext.HttpContext.Response.Clear();
                filterContext.RequestContext.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}