using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;

namespace TicketManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorLoggerAttribute());
        }
    }
}
