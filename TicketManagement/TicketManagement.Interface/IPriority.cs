using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IPriority
    {
        List<Priority> GetAllPriority();
        List<SelectListItem> GetAllPrioritySelectListItem();
    }
}
