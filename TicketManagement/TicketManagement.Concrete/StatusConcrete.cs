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

namespace TicketManagement.Concrete
{
    public class StatusConcrete : IStatus
    {
        private readonly DatabaseContext _context;
        public StatusConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetAllStatusSelectListItem()
        {
            try
            {
                List<SelectListItem> statusList;
                string key = "Status_Cache";
                if (!CacheHelper.CheckExists(key))
                {
                    statusList = (from status in _context.Status
                                  select new SelectListItem()
                                  {
                                      Text = status.StatusText,
                                      Value = status.StatusId.ToString()
                                  }).ToList();


                    statusList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    CacheHelper.AddToCacheWithNoExpiration(key, statusList);
                }
                else
                {
                    statusList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return statusList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        
            
        public List<SelectListItem> GetAllStatusWithoutOverdueandEscalationSelectListItem()
        {
            try
            {
                List<SelectListItem> statusList;
                string key = "CommonStatus_Cache";

                if (!CacheHelper.CheckExists(key))
                {
                    statusList = (from status in _context.Status
                                  where status.IsInternalStatus == false
                        select new SelectListItem()
                        {
                            Text = status.StatusText,
                            Value = status.StatusId.ToString()
                        }).ToList();


                    statusList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    CacheHelper.AddToCacheWithNoExpiration(key, statusList);
                }
                else
                {
                    statusList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return statusList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
