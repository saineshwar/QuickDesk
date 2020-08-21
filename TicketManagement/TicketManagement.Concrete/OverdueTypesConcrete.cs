using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;

namespace TicketManagement.Concrete
{
    public class OverdueTypesConcrete : IOverdueTypes
    {
        private readonly DatabaseContext _context;
        public OverdueTypesConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetAllActiveOverdueTypes()
        {
            try
            {
                var overdueTypeList = (from cat in _context.OverdueTypes
                    select new SelectListItem()
                                       {
                                           Text = cat.OverdueType,
                                           Value = cat.OverdueTypeId.ToString()
                                       }).ToList();

                overdueTypeList.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return overdueTypeList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
