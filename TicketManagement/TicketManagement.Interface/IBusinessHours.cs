using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IBusinessHours
    {
        int? AddBusinessHours( BusinessHours businessHours, List<BusinessHoursDetails> listBusinessHoursDetails);
        int? AddBusinessHours(BusinessHours businessHours);
        List<SelectListItem> ListofBusinessHoursType();
        int GetBusinessHoursCount(string name);
        List<BusinessHoursViewModel> GetBusinessList(string name, int startIndex, int count, string sorting);
        int DeleteBusinessHours(int? businessHoursId);
        List<BusinessHoursDetails> DetailsBusinessHours(int? businessHoursId);
        List<SelectListItem> ListofBusinessHours();
    }
}
