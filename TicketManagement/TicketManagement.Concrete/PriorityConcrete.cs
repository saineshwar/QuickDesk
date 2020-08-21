using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class PriorityConcrete : IPriority
    {
        private readonly DatabaseContext _context;
        public PriorityConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<Priority> GetAllPriority()
        {
            try
            {
                var priorityList = (from priority in _context.Priority
                                    select priority).ToList();
                return priorityList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllPrioritySelectListItem()
        {
            try
            {
                List<SelectListItem> priorityList;
                string key = "Priority_Cache";
                if (!CacheHelper.CheckExists(key))
                {
                    priorityList = (from priority in _context.Priority
                                    select new SelectListItem()
                                    {
                                        Text = priority.PriorityName,
                                        Value = priority.PriorityId.ToString()
                                    }).ToList();


                    priorityList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    CacheHelper.AddToCacheWithNoExpiration(key, priorityList);

                }
                else
                {
                    priorityList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return priorityList;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
