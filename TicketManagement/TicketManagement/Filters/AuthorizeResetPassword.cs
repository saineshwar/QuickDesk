using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagement.Filters
{
    public class AuthorizeResetPassword : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session["ActiveVerification"])))
            {
                string activeVerificationvalue = (string)context.HttpContext.Session["ActiveVerification"];

                if ("1" != activeVerificationvalue)
                {
                    ViewResult result = new ViewResult();
                    result.ViewName = "Error";
                    context.Result = result;
                }
            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";
                context.Result = result;

            }
        }
    }
}