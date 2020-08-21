using System.Collections.Generic;
using System.Web.Mvc;

namespace TicketManagement.Interface
{
    public interface IOverdueTypes
    {
        List<SelectListItem> GetAllActiveOverdueTypes();
    }
}