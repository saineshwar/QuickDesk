using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class AllTicketGridConcrete : IAllTicketGrid
    {
        private readonly DatabaseContext _context;

        public AllTicketGridConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int AllAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@search", search);
                    param.Add("@searchin", searchin);
                    param.Add("@prioritysearch", prioritysearch);
                    param.Add("@statussearch", statussearch);
                    return con.Query<int>("Usp_AllAdminTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> AllAdminTickets(string search, string searchin, int? prioritysearch,
            int? statussearch, int startIndex, int count, string sorting)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = _context.Tickets
                        .Join(_context.TicketStatus, t => t.TicketId, ts => ts.TicketId,
                            (t, ts) => new { t1 = t, ticketStatus = ts })

                        .Join(_context.TicketDetails, t => t.t1.TicketId, td => td.TicketId,
                            (t, td) => new { t2 = t, ticketDetails = td })

                        .Join(_context.Status, t => t.t2.ticketStatus.StatusId, st => st.StatusId,
                            (t, st) => new { t3 = t, status = st })

                        .Join(_context.Priority, t => t.t3.t2.ticketStatus.PriorityId, p => p.PriorityId,
                            (t, p) => new { t4 = t, priority = p })
                        .Join(_context.Category, t => t.t4.t3.t2.t1.CategoryId, c => c.CategoryId,
                            (t, c) => new { t5 = t, Category = c })

                        .Join(_context.TicketDeleteLockStatus, t => t.t5.t4.t3.t2.t1.TicketId, dl => dl.TicketId,
                            (t, dl) => new { t6 = t, deletelock = dl })

                        .Select(x => new ViewTicketModel
                        {
                            TicketId = x.t6.t5.t4.t3.t2.t1.TicketId,
                            Name = x.t6.t5.t4.t3.t2.t1.Name,
                            Category = x.t6.Category.CategoryName,
                            Subject = x.t6.t5.t4.t3.ticketDetails.Subject,
                            TrackingId = x.t6.t5.t4.t3.t2.t1.TrackingId,
                            Status = x.t6.t5.t4.status.StatusText,
                            Priority = x.t6.t5.priority.PriorityName,
                            StatusId = x.t6.t5.t4.status.StatusId,
                            PriorityId = x.t6.t5.priority.PriorityId,
                            StatusAssigned = x.t6.t5.t4.t3.t2.t1.StatusAssigned,
                            UserId = x.t6.t5.t4.t3.t2.ticketStatus.UserId,
                            TicketUpdatedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketUpdatedDate,
                            TicketAssignedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketAssignedDate,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive,
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId,
                            FirstResponseDue = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseDue,
                            FirstResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseStatus,
                            ResolutionDue = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionDue,
                            ResolutionStatus = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionStatus,
                            EveryResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.EveryResponseStatus,
                            EscalationStatus = x.t6.t5.t4.t3.t2.ticketStatus.EscalationStatus

                        });


                    if (statussearch == 7)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == true
                        );
                    }
                    else if (statussearch == 8)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == true || t.ResolutionStatus == true || t.EveryResponseStatus == true)
                                                 && t.EscalationStatus == false);
                    }
                    else if (statussearch == 9)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && t.EscalationStatus == true
                        );
                    }

                    if (searchin == "1" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.TrackingId == search);
                    }
                    else if (searchin == "2" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Name == search);
                    }
                    else if (searchin == "3" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Subject == search);
                    }
                    else if (searchin == "4" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Category == search);
                    }

                    if (prioritysearch != null)
                    {
                        query = query.Where(p => p.PriorityId == prioritysearch);
                    }

                    if (statussearch != null && statussearch != 8 && statussearch != 9)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }

                    if (sorting != null && sorting.Equals("TicketId ASC"))
                    {
                        query = query.OrderBy(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketId DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate ASC"))
                    {
                        query = query.OrderBy(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("Category ASC"))
                    {
                        query = query.OrderBy(p => p.Category);
                    }
                    else if (sorting != null && sorting.Equals("Category DESC"))
                    {
                        query = query.OrderByDescending(p => p.Category);
                    }
                    else if (sorting != null  && sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting != null && sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AllAgentsTicketsCount(long userId, string search, string searchin, int? prioritysearch,
            int? statussearch)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@search", search);
                    param.Add("@searchin", searchin);
                    param.Add("@prioritysearch", prioritysearch);
                    param.Add("@statussearch", statussearch);
                    return con.Query<int>("Usp_AllAgentsTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> AllAgentsTickets(long userId, string search, string searchin,
         int? prioritysearch, int? statussearch, int startIndex, int count, string sorting)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = _context.Tickets
                        .Join(_context.TicketStatus, t => t.TicketId, ts => ts.TicketId,
                            (t, ts) => new { t1 = t, ticketStatus = ts })

                        .Join(_context.TicketDetails, t => t.t1.TicketId, td => td.TicketId,
                            (t, td) => new { t2 = t, ticketDetails = td })

                        .Join(_context.Status, t => t.t2.ticketStatus.StatusId, st => st.StatusId,
                            (t, st) => new { t3 = t, status = st })

                        .Join(_context.Priority, t => t.t3.t2.ticketStatus.PriorityId, p => p.PriorityId,
                            (t, p) => new { t4 = t, priority = p })

                        .Join(_context.Category, t => t.t4.t3.t2.t1.CategoryId, c => c.CategoryId,
                            (t, c) => new { t5 = t, Category = c })

                        .Join(_context.TicketDeleteLockStatus, t => t.t5.t4.t3.t2.t1.TicketId, dl => dl.TicketId,
                            (t, dl) => new { t6 = t, deletelock = dl })

                        .Select(x => new ViewTicketModel
                        {
                            TicketId = x.t6.t5.t4.t3.t2.t1.TicketId,
                            Name = x.t6.t5.t4.t3.t2.t1.Name,
                            Category = x.t6.Category.CategoryName,
                            Subject = x.t6.t5.t4.t3.ticketDetails.Subject,
                            TrackingId = x.t6.t5.t4.t3.t2.t1.TrackingId,
                            Status = x.t6.t5.t4.status.StatusText,
                            Priority = x.t6.t5.priority.PriorityName,
                            StatusId = x.t6.t5.t4.status.StatusId,
                            PriorityId = x.t6.t5.priority.PriorityId,
                            StatusAssigned = x.t6.t5.t4.t3.t2.t1.StatusAssigned,
                            UserId = x.t6.t5.t4.t3.t2.ticketStatus.UserId,
                            TicketUpdatedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketUpdatedDate,
                            TicketAssignedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketAssignedDate,
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive,
                            FirstResponseDue = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseDue,
                            FirstResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseStatus,
                            ResolutionDue = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionDue,
                            ResolutionStatus = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionStatus,
                            EveryResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.EveryResponseStatus,
                            EscalationStatus = x.t6.t5.t4.t3.t2.ticketStatus.EscalationStatus,
                        });

                    if (statussearch == 7)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == true
                                                 && t.UserId == userId
                                                 );
                    }
                    else if (statussearch == 8)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == true || t.ResolutionStatus == true || t.EveryResponseStatus == true)
                                                 && t.EscalationStatus == false
                                                 && t.UserId == userId);
                    }
                    else if (statussearch == 9)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && t.EscalationStatus == true
                                                 && t.UserId == userId
                                                 );
                    }
                    else if (statussearch == 10)
                    {
                        query = query.Where(t => t.IsActive == true);
                    }
                    else
                    {
                        query = query.Where(t => t.UserId == userId);
                    }



                    if (searchin == "1" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.TrackingId == search);
                    }
                    else if (searchin == "2" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Name == search);
                    }
                    else if (searchin == "3" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Subject == search);
                    }
                    else if (searchin == "4" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Category == search);
                    }

                    if (prioritysearch != null)
                    {
                        query = query.Where(p => p.PriorityId == prioritysearch);
                    }
                    if (statussearch != null && statussearch != 8 && statussearch != 9)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }

                    if (!string.IsNullOrEmpty(sorting) && sorting.Equals("TicketId ASC"))
                    {
                        query = query.OrderBy(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketId DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting != null && sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else if (sorting != null && sorting.Equals("Category ASC"))
                    {
                        query = query.OrderBy(p => p.Category);
                    }
                    else if (sorting != null && sorting.Equals("Category DESC"))
                    {
                        query = query.OrderByDescending(p => p.Category);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate ASC"))
                    {
                        query = query.OrderBy(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate);
                    }
                    else
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AllAgentAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch, int categoryId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@search", search);
                    param.Add("@searchin", searchin);
                    param.Add("@prioritysearch", prioritysearch);
                    param.Add("@statussearch", statussearch);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<int>("Usp_AllAgentAdminTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> AllAgentAdminTicketsbyList(string search, string searchin, int categoryId,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = _context.Tickets
                        .Join(_context.TicketStatus, t => t.TicketId, ts => ts.TicketId,
                            (t, ts) => new { t1 = t, ticketStatus = ts })

                        .Join(_context.TicketDetails, t => t.t1.TicketId, td => td.TicketId,
                            (t, td) => new { t2 = t, ticketDetails = td })

                        .Join(_context.Status, t => t.t2.ticketStatus.StatusId, st => st.StatusId,
                            (t, st) => new { t3 = t, status = st })

                        .Join(_context.Priority, t => t.t3.t2.ticketStatus.PriorityId, p => p.PriorityId,
                            (t, p) => new { t4 = t, priority = p })

                        .Join(_context.Category, t => t.t4.t3.t2.t1.CategoryId, c => c.CategoryId,
                            (t, c) => new { t5 = t, Category = c })

                        .Join(_context.TicketDeleteLockStatus, t => t.t5.t4.t3.t2.t1.TicketId, dl => dl.TicketId,
                            (t, dl) => new { t6 = t, deletelock = dl })

                        .Select(x => new ViewTicketModel
                        {
                            TicketId = x.t6.t5.t4.t3.t2.t1.TicketId,
                            Name = x.t6.t5.t4.t3.t2.t1.Name,
                            Category = x.t6.Category.CategoryName,
                            Subject = x.t6.t5.t4.t3.ticketDetails.Subject,
                            TrackingId = x.t6.t5.t4.t3.t2.t1.TrackingId,
                            Status = x.t6.t5.t4.status.StatusText,
                            Priority = x.t6.t5.priority.PriorityName,
                            StatusId = x.t6.t5.t4.status.StatusId,
                            PriorityId = x.t6.t5.priority.PriorityId,
                            StatusAssigned = x.t6.t5.t4.t3.t2.t1.StatusAssigned,
                            UserId = x.t6.t5.t4.t3.t2.ticketStatus.UserId,
                            TicketUpdatedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketUpdatedDate,
                            TicketAssignedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketAssignedDate,
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive,
                            FirstResponseDue = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseDue,
                            FirstResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseStatus,
                            ResolutionDue = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionDue,
                            ResolutionStatus = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionStatus,
                            EveryResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.EveryResponseStatus,
                            EscalationStatus = x.t6.t5.t4.t3.t2.ticketStatus.EscalationStatus
                        });



                    if (statussearch == 7)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == true
                                                 && t.CategoryId == categoryId);
                    }
                    else if (statussearch == 8)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == true || t.ResolutionStatus == true || t.EveryResponseStatus == true)
                                                 && t.EscalationStatus == false
                                                 && t.CategoryId == categoryId);
                    }
                    else if (statussearch == 9)
                    {
                        query = query.Where(t => t.IsActive == true
                                                  && t.DeleteStatus == false
                                                  && t.EscalationStatus == true
                                                  && t.CategoryId == categoryId
                        );
                    }
                    else if (statussearch == 10)
                    {
                        query = query.Where(t => t.IsActive == true);
                    }
                    else
                    {
                        query = query.Where(t => t.CategoryId == categoryId);
                    }


                    if (searchin == "1" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.TrackingId == search);
                    }
                    else if (searchin == "2" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Name == search);
                    }
                    else if (searchin == "3" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Subject == search);
                    }
                    else if (searchin == "4" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Category == search);
                    }

                    if (prioritysearch != null)
                    {
                        query = query.Where(p => p.PriorityId == prioritysearch);
                    }

                    if (statussearch != null && statussearch != 8 && statussearch != 9)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }

                    if (sorting != null&&sorting.Equals("TicketId ASC"))
                    {
                        query = query.OrderBy(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketId DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate ASC"))
                    {
                        query = query.OrderBy(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("Category ASC"))
                    {
                        query = query.OrderBy(p => p.Category);
                    }
                    else if (sorting != null && sorting.Equals("Category DESC"))
                    {
                        query = query.OrderByDescending(p => p.Category);
                    }
                    else if (sorting != null  && sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting != null && sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AllHodTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch, int categoryId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@search", search);
                    param.Add("@searchin", searchin);
                    param.Add("@prioritysearch", prioritysearch);
                    param.Add("@statussearch", statussearch);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<int>("Usp_AllHODTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> AllHodTicketsbyList(string search, string searchin, int categoryId,
         int? prioritysearch, int? statussearch, int startIndex, int count, string sorting)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var query = _context.Tickets
                        .Join(_context.TicketStatus, t => t.TicketId, ts => ts.TicketId,
                            (t, ts) => new { t1 = t, ticketStatus = ts })

                        .Join(_context.TicketDetails, t => t.t1.TicketId, td => td.TicketId,
                            (t, td) => new { t2 = t, ticketDetails = td })

                        .Join(_context.Status, t => t.t2.ticketStatus.StatusId, st => st.StatusId,
                            (t, st) => new { t3 = t, status = st })

                        .Join(_context.Priority, t => t.t3.t2.ticketStatus.PriorityId, p => p.PriorityId,
                            (t, p) => new { t4 = t, priority = p })

                        .Join(_context.Category, t => t.t4.t3.t2.t1.CategoryId, c => c.CategoryId,
                            (t, c) => new { t5 = t, Category = c })

                        .Join(_context.TicketDeleteLockStatus, t => t.t5.t4.t3.t2.t1.TicketId, dl => dl.TicketId,
                            (t, dl) => new { t6 = t, deletelock = dl })

                        .Select(x => new ViewTicketModel
                        {
                            TicketId = x.t6.t5.t4.t3.t2.t1.TicketId,
                            Name = x.t6.t5.t4.t3.t2.t1.Name,
                            Category = x.t6.Category.CategoryName,
                            Subject = x.t6.t5.t4.t3.ticketDetails.Subject,
                            TrackingId = x.t6.t5.t4.t3.t2.t1.TrackingId,
                            Status = x.t6.t5.t4.status.StatusText,
                            Priority = x.t6.t5.priority.PriorityName,
                            StatusId = x.t6.t5.t4.status.StatusId,
                            PriorityId = x.t6.t5.priority.PriorityId,
                            StatusAssigned = x.t6.t5.t4.t3.t2.t1.StatusAssigned,
                            UserId = x.t6.t5.t4.t3.t2.ticketStatus.UserId,
                            TicketUpdatedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketUpdatedDate,
                            TicketAssignedDate = x.t6.t5.t4.t3.t2.ticketStatus.TicketAssignedDate,
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive,
                            FirstResponseDue = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseDue,
                            FirstResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.FirstResponseStatus,
                            ResolutionDue = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionDue,
                            ResolutionStatus = x.t6.t5.t4.t3.t2.ticketStatus.ResolutionStatus,
                            EveryResponseStatus = x.t6.t5.t4.t3.t2.ticketStatus.EveryResponseStatus,
                            EscalationStatus = x.t6.t5.t4.t3.t2.ticketStatus.EscalationStatus
                        });



                    if (statussearch == 7)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == true
                                                 && t.CategoryId == categoryId);
                    }
                    else if (statussearch == 8)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == true || t.ResolutionStatus == true || t.EveryResponseStatus == true)
                                                 && t.EscalationStatus == false
                                                 && t.CategoryId == categoryId);
                    }
                    else if (statussearch == 9)
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && t.EscalationStatus == true
                                                 && t.CategoryId == categoryId
                        );
                    }
                    else if (statussearch == 10)
                    {
                        query = query.Where(t => t.IsActive == true);
                    }
                    else
                    {
                        query = query.Where(t => t.CategoryId == categoryId);
                    }


                    if (searchin == "1" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.TrackingId == search);
                    }
                    else if (searchin == "2" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Name == search);
                    }
                    else if (searchin == "3" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Subject == search);
                    }
                    else if (searchin == "4" && !string.IsNullOrEmpty(search))
                    {
                        query = query.Where(p => p.Category == search);
                    }

                    if (prioritysearch != null)
                    {
                        query = query.Where(p => p.PriorityId == prioritysearch);
                    }

                    if (statussearch != null && statussearch != 8 && statussearch != 9)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }

                    if (sorting != null && sorting.Equals("TicketId ASC"))
                    {
                        query = query.OrderBy(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketId DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketId);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate ASC"))
                    {
                        query = query.OrderBy(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("TicketUpdatedDate DESC"))
                    {
                        query = query.OrderByDescending(p => p.TicketUpdatedDate);
                    }
                    else if (sorting != null && sorting.Equals("Category ASC"))
                    {
                        query = query.OrderBy(p => p.Category);
                    }
                    else if (sorting != null && sorting.Equals("Category DESC"))
                    {
                        query = query.OrderByDescending(p => p.Category);
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
                        query = query.OrderByDescending(p => p.TicketUpdatedDate); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AllUserTicketsCount(long userId, string search, int? statusId, string searchin)
        {
            using (SqlConnection con =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@search", search);
                param.Add("@searchin", searchin);
                param.Add("@statusId", statusId);
                return con.Query<int>("Usp_AllUsersTicketsCount", param, null, false, 0,
                    CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public List<ViewTicketModel> AllUserTickets(string search, string searchin, int? statusId, long userId, int startIndex, int count, string sorting)
        {

            var query = (from tickets in _context.Tickets
                         join ticketStatus in _context.TicketStatus on tickets.TicketId equals ticketStatus.TicketId
                             into ticketStatusgroup
                         from t in ticketStatusgroup.DefaultIfEmpty()

                         join category in _context.Category on tickets.CategoryId equals category.CategoryId
                         join ticketdetails in _context.TicketDetails on tickets.TicketId equals ticketdetails
                             .TicketId
                         join status in _context.Status on t.StatusId equals status.StatusId into statusgroup
                         from s in statusgroup.DefaultIfEmpty()
                         join priority in _context.Priority on t.PriorityId equals priority.PriorityId into
                             prioritygroup
                         from p in prioritygroup.DefaultIfEmpty()
                         join dl in _context.TicketDeleteLockStatus on tickets.TicketId equals dl.TicketId

                         select new ViewTicketModel()
                         {
                             TicketId = tickets.TicketId,
                             Name = tickets.Name,
                             Category = category.CategoryName,
                             Subject = ticketdetails.Subject,
                             TrackingId = tickets.TrackingId,
                             Status = string.IsNullOrEmpty(s.StatusText) ? "Assigning Ticket.. " : s.StatusText,
                             Priority = string.IsNullOrEmpty(p.PriorityName) ? "Assigning Ticket.." : p.PriorityName,
                             DeleteStatus = dl.TicketDeleteStatus,
                             TicketAssignedDate = t.TicketAssignedDate,
                             TicketUpdatedDate = t.TicketUpdatedDate,
                             StatusId = t.StatusId,
                             UserId = tickets.UserId,
                             FirstResponseStatus = t.FirstResponseStatus,
                             FirstResponseDue = t.FirstResponseDue,
                             ResolutionStatus = t.ResolutionStatus,
                             ResolutionDue = t.ResolutionDue,
                             EveryResponseStatus = t.EveryResponseStatus,
                             EscalationStatus = t.EscalationStatus,
                             IsActive = t.IsActive,
                             CategoryId = t.CategoryId,
                             PriorityId = t.PriorityId
                         });


            if (statusId == 1)
            {
                query = query.Where(p => p.UserId == userId
                    && p.DeleteStatus == false
                    && (p.FirstResponseStatus == false && p.ResolutionStatus == false && p.EveryResponseStatus == false)
                    && p.EscalationStatus == false
                    && p.StatusId == statusId || p.StatusId == null);
            }
            else if (statusId == 7)
            {
                query = query.Where(p => p.UserId == userId && p.IsActive == true && p.DeleteStatus == true);
            }
            else if (statusId == 8)
            {
                query = query.Where(p => p.UserId == userId && p.IsActive == true
                                                            && p.DeleteStatus == false
                                                            && (p.FirstResponseStatus == true || p.ResolutionStatus == true || p.EveryResponseStatus == true)
                                                            && p.EscalationStatus == false
                                         );
            }
            else if (statusId == 9)
            {
                query = query.Where(p => p.UserId == userId && p.IsActive == true
                                                            && p.DeleteStatus == false
                                                            && p.EscalationStatus == true);
            }
            else
            {
                query = query.Where(p => p.UserId == userId);
            }

            if (searchin == "1" && !string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.TrackingId == search);
            }
            else if (searchin == "2" && !string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name == search);
            }
            else if (searchin == "3" && !string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Subject == search);
            }
            if (sorting != null && sorting.Equals("TicketId ASC"))
            {
                query = query.OrderBy(p => p.TicketId);
            }
            else if (sorting != null && sorting.Equals("TicketId DESC"))
            {
                query = query.OrderByDescending(p => p.TicketId);
            }
            else if (sorting != null && sorting.Equals("Name ASC"))
            {
                query = query.OrderBy(p => p.Name);
            }
            else if (sorting != null && sorting.Equals("Name DESC"))
            {
                query = query.OrderByDescending(p => p.Name);
            }
            else if (sorting != null && sorting.Equals("Priority ASC"))
            {
                query = query.OrderBy(p => p.Priority);
            }
            else if (sorting != null && sorting.Equals("Priority DESC"))
            {
                query = query.OrderByDescending(p => p.Priority);
            }
            else if (sorting != null && sorting.Equals("Status ASC"))
            {
                query = query.OrderBy(p => p.Status);
            }
            else if (sorting != null && sorting.Equals("Status DESC"))
            {
                query = query.OrderByDescending(p => p.Status);
            }
            else if (sorting != null && sorting.Equals("Category ASC"))
            {
                query = query.OrderBy(p => p.Category);
            }
            else if (sorting != null && sorting.Equals("Category DESC"))
            {
                query = query.OrderByDescending(p => p.Category);
            }
            else
            {
                query = query.OrderByDescending(p => p.TicketUpdatedDate); //Default!
            }

            return count > 0
                ? query.Skip(startIndex).Take(count).ToList() //Paging
                : query.ToList(); //No paging

        }
    }
}