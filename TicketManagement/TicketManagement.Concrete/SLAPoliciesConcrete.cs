using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class SlaPoliciesConcrete : ISlaPolicies
    {
        private readonly DatabaseContext _context;

        public SlaPoliciesConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddSlaPolicies(SlaPolicies slaPolicies)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SlaPolicies.Add(slaPolicies);
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

        public bool CheckPoliciesExists(int? priorityId)
        {
            try
            {
                var result = (from slaPolicy in _context.SlaPolicies
                              where slaPolicy.PriorityId == priorityId
                              select slaPolicy).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSlaPoliciesCount(string priorityName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(priorityName))
                    {
                        var data = (from slaPoliciese in db.SlaPolicies
                                    join priority in db.Priority on slaPoliciese.PriorityId equals priority.PriorityId
                                    where priority.PriorityName == priorityName
                                    select slaPoliciese).Count();

                        return data;
                    }
                    else
                    {
                        var data = (from slaPoliciese in db.SlaPolicies
                                    select slaPoliciese).Count();
                        return data;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SlaPoliciesShowViewModel> GetSlaPoliciesList(string priorityName, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = (from slaPoliciese in db.SlaPolicies
                                 join priority in db.Priority on slaPoliciese.PriorityId equals priority.PriorityId
                                 select new SlaPoliciesShowViewModel()
                                 {
                                     EveryResponseDay = slaPoliciese.EveryResponseDay,
                                     EveryResponseHour = slaPoliciese.EveryResponseHour,
                                     EveryResponseMins = slaPoliciese.EveryResponseMins,
                                     PriorityName = priority.PriorityName,
                                     FirstResponseDay = slaPoliciese.FirstResponseDay,
                                     FirstResponseHour = slaPoliciese.FirstResponseHour,
                                     FirstResponseMins = slaPoliciese.FirstResponseMins,
                                     ResolutionResponseDay = slaPoliciese.ResolutionResponseDay,
                                     ResolutionResponseHour = slaPoliciese.ResolutionResponseHour,
                                     ResolutionResponseMins = slaPoliciese.ResolutionResponseMins,
                                     SlaPoliciesId = slaPoliciese.SlaPoliciesId,
                                     EscalationDay = slaPoliciese.EscalationDay,
                                     EscalationHour = slaPoliciese.EscalationHour,
                                     EscalationMins = slaPoliciese.EscalationMins
                                 }).AsQueryable();

                    if (!string.IsNullOrEmpty(priorityName))
                    {
                        query = query.Where(p => p.PriorityName == priorityName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("SlaPoliciesId ASC"))
                    {
                        query = query.OrderBy(p => p.SlaPoliciesId);
                    }
                    else if (sorting.Equals("SlaPoliciesId DESC"))
                    {
                        query = query.OrderByDescending(p => p.SlaPoliciesId);
                    }
                    else if (sorting.Equals("PriorityName ASC"))
                    {
                        query = query.OrderBy(p => p.PriorityName);
                    }
                    else if (sorting.Equals("PriorityName DESC"))
                    {
                        query = query.OrderByDescending(p => p.PriorityName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.SlaPoliciesId); //Default!
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

        public int DeleteSlaPolicies(int? slaPoliciesId)
        {

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var slaPolicies = _context.SlaPolicies.Find(slaPoliciesId);
                    if (slaPolicies != null)
                        _context.SlaPolicies.Remove(slaPolicies);
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
}