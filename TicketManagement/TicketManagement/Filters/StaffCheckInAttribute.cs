using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagement.CommonData;
using TicketManagement.Concrete;
using TicketManagement.Helpers;

namespace TicketManagement.Filters
{
    public class AgentCheckInAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                SessionHandler sessionHandler = new SessionHandler();
                var userId = Convert.ToString(sessionHandler.UserId);

                if (!string.IsNullOrEmpty(userId))
                {
                    var tempUserId = Convert.ToInt64(sessionHandler.UserId);

                    bool checkStatus;
                    using (var context = new DatabaseContext())
                    {
                        checkStatus = (from a in context.AgentCheckInStatusSummary
                                       where a.UserId == tempUserId
                                       select a.AgentStatus).FirstOrDefault();
                    }
                    if (!checkStatus)
                    {
                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary
                            (new
                            { controller = "CheckInAlert", action = "Alerts" }
                            ));
                    }

                }
                else
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