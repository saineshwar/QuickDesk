using System.Collections.Generic;
using System.Web.Mvc;

namespace TicketManagement.Interface
{
    public interface IStatus
    {
        List<SelectListItem> GetAllStatusSelectListItem();
        List<SelectListItem> GetAllStatusWithoutOverdueandEscalationSelectListItem();
    }
}