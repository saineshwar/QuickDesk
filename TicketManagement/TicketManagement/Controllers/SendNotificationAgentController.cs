using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagement.Common.Emails;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    public class SendNotificationAgentController : Controller
    {
        private IProcessSettings _processSettings;
        public SendNotificationAgentController(IProcessSettings processSettings)
        {
            _processSettings = processSettings;
        }

        // GET: SendNotificationAgent
        public ActionResult Notification(RequestTicketEmailViewModel requestTicketEmailViewModel)
        {

            if (!string.IsNullOrEmpty(requestTicketEmailViewModel.TrackingId))
            {
                SendingEmailhelper sendingEmailhelper = new SendingEmailhelper();
                sendingEmailhelper.Send(requestTicketEmailViewModel.TrackingId);
            }

            var controller = RouteData.Values["controller"].ToString();
            var action = RouteData.Values["action"].ToString();

            if (Request.UrlReferrer != null)
            {
                var fullUrl = Request.UrlReferrer.ToString();
                var questionMarkIndex = fullUrl.IndexOf('?');
                string queryString = null;
                string url = fullUrl;
                if (questionMarkIndex != -1) // There is a QueryString
                {
                    url = fullUrl.Substring(0, questionMarkIndex);
                    queryString = fullUrl.Substring(questionMarkIndex + 1);
                }

                // Arranges
                var request = new HttpRequest(null, url, queryString);
                var response = new HttpResponse(new StringWriter());
                var httpContext = new HttpContext(request, response);

                var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

                // Extract the data    
                if (routeData != null)
                {
                    var values = routeData.Values;
                    var controllerName = values["controller"];
                    var actionName = values["action"];
                    var areaName = values["area"];

                    return RedirectToAction(actionName.ToString(), controllerName.ToString(), new { trackingId = requestTicketEmailViewModel.TrackingId });
                }


            }

            return RedirectToAction("Login", "Login");
        }
    }
}