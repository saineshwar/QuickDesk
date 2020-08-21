using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class BusinessHoursConcrete : IBusinessHours
    {
        private readonly DatabaseContext _context;

        public BusinessHoursConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddBusinessHours(BusinessHours businessHours, List<BusinessHoursDetails> listBusinessHoursDetails)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    _context.BusinessHours.Add(businessHours);
                    _context.SaveChanges();

                    var businessHoursId = businessHours.BusinessHoursId;

                    foreach (var businessHoursDetail in listBusinessHoursDetails)
                    {
                        businessHoursDetail.BusinessHoursId = businessHoursId;
                        _context.BusinessHoursDetails.Add(businessHoursDetail);
                        _context.SaveChanges();
                    }



                    dbContextTransaction.Commit();
                    return 1;
                }
                catch (Exception)
                {

                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int? AddBusinessHours(BusinessHours businessHours)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.BusinessHours.Add(businessHours);
                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                    return 1;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }


        public List<SelectListItem> ListofBusinessHoursType()
        {
            var listbt = from bussinesstype in _context.BusinessHoursType
                         select new SelectListItem()
                         {
                             Text = bussinesstype.BusinessHoursName,
                             Value = bussinesstype.BusinessHoursTypeId.ToString()
                         };

            return listbt.ToList();
        }

        public int GetBusinessHoursCount(string name)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        var data = (from bhd in db.BusinessHours
                                    where bhd.Name == name
                                    select bhd).Count();

                        return data;
                    }
                    else
                    {
                        var data = (from bhd in db.BusinessHours
                                    select bhd).Count();
                        return data;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BusinessHoursViewModel> GetBusinessList(string name, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = (from bh in db.BusinessHours
                                 join bt in db.BusinessHoursType on bh.HelpdeskHoursType equals bt.BusinessHoursTypeId
                                 select new BusinessHoursViewModel()
                                 {
                                     Name = bh.Name,
                                     Description = bh.Description,
                                     BusinessHoursName = bt.BusinessHoursName,
                                     CreateDate = bh.CreateDate,
                                     BusinessHoursId = bh.BusinessHoursId
                                 }).AsQueryable();

                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.Name == name);
                    }

                    //Sorting Ascending and Descending
                    if (sorting != null && sorting.Equals("BusinessHoursId ASC"))
                    {
                        query = query.OrderBy(p => p.BusinessHoursId);
                    }
                    else if (sorting != null && sorting.Equals("BusinessHoursId DESC"))
                    {
                        query = query.OrderByDescending(p => p.BusinessHoursId);
                    }
                    else if (sorting != null && sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting != null && sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.BusinessHoursId); //Default!
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

        public int DeleteBusinessHours(int? businessHoursId)
        {
            var businessHourscount = (from bh in _context.BusinessHours
                                      where bh.BusinessHoursId == businessHoursId
                                      select bh).Count();
            if (businessHourscount > 0)
            {
                return 0;
            }
            else
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var businessHours = _context.BusinessHours.Find(businessHoursId);
                        if (businessHours != null)
                            _context.BusinessHours.Remove(businessHours);
                        var businessHoursdetails = _context.BusinessHoursDetails.Find(businessHoursId);
                        if (businessHoursdetails != null) _context.BusinessHoursDetails.Remove(businessHoursdetails);
                        _context.SaveChanges();
                        dbContextTransaction.Commit();

                        return 1;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return -1;
                    }
                }
            }

        }

        public List<BusinessHoursDetails> DetailsBusinessHours(int? businessHoursId)
        {
            var businessHourslist = (from bh in _context.BusinessHoursDetails
                                     where bh.BusinessHoursId == businessHoursId
                                     select bh).ToList();
            return businessHourslist;
        }


        public List<SelectListItem> ListofBusinessHours()
        {
            var listbt = (from bussinesstype in _context.BusinessHours
                          select new SelectListItem()
                          {
                              Text = bussinesstype.Name,
                              Value = bussinesstype.BusinessHoursId.ToString()
                          }).ToList();


            listbt.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listbt.ToList();
        }
    }
}