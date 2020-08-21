using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using TicketManagement.Interface;
using TicketManagement.Models;
using System.Linq;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class HolidayListConcrete : IHolidayList
    {
        private readonly DatabaseContext _context;
        public HolidayListConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddHoliday(HolidayList holiday)
        {
            try
            {
                int? result = -1;

                if (holiday != null)
                {
                    holiday.CreatedDate = DateTime.Now;
                    _context.HolidayList.Add(holiday);
                    _context.SaveChanges();
                    result = holiday.HolidayId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetHolidayCount(string hoilday)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(hoilday))
                    {
                        var data = (from holiday in db.HolidayList
                                    where holiday.HolidayName == hoilday
                                    select holiday).Count();

                        return data;
                    }
                    else
                    {
                        var data = (from holiday in db.HolidayList
                                    select holiday).Count();
                        return data;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HolidayList> GetHolidayList(string holidayName, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = (from holiday in db.HolidayList
                        select holiday).AsQueryable();

                    if (!string.IsNullOrEmpty(holidayName))
                    {
                        query = query.Where(p =>  p.HolidayName == holidayName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("HolidayId ASC"))
                    {
                        query = query.OrderBy(p => p.HolidayId);
                    }
                    else if (sorting.Equals("HolidayId DESC"))
                    {
                        query = query.OrderByDescending(p => p.HolidayId);
                    }
                    else if (sorting.Equals("HolidayName ASC"))
                    {
                        query = query.OrderBy(p => p.HolidayName);
                    }
                    else if (sorting.Equals("HolidayName DESC"))
                    {
                        query = query.OrderByDescending(p => p.HolidayName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.HolidayId); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteHoliday(int? holidayId)
        {
            try
            {
                var holiday = _context.HolidayList.Find(holidayId);
                if (holiday != null)
                    _context.HolidayList.Remove(holiday);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}