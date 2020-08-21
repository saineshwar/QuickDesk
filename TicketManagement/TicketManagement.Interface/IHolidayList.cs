using System.Collections.Generic;
using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IHolidayList
    {
        int? AddHoliday(HolidayList holiday);
        int GetHolidayCount(string hoilday);
        List<HolidayList> GetHolidayList(string holidayName, int startIndex, int count, string sorting);
        void DeleteHoliday(int? holidayId);
    }
}