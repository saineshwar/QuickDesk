using System;
using System.Web;
using System.Web.Mvc;


public static class HtmlHelperExtensions
{
    public static string IsActive(this HtmlHelper html, string controller = null, string action = null)
    {
        string activeClass = "nav-sm";
        string sessionState = Convert.ToString(HttpContext.Current.Session["ToggleState"]);
        if (string.IsNullOrEmpty(sessionState))
        {
            HttpContext.Current.Session["ToggleState"] = activeClass;
            return activeClass;
        }
        else
        {
            if (sessionState == "nav-sm")
            {
                HttpContext.Current.Session["ToggleState"] = "nav-md";
                return Convert.ToString(HttpContext.Current.Session["ToggleState"]);
            }
            else
            {
                HttpContext.Current.Session["ToggleState"] = "nav-sm";
                return Convert.ToString(HttpContext.Current.Session["ToggleState"]);
            }
        }
    }
}
