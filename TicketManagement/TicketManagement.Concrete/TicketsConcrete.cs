using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class TicketsConcrete : ITickets
    {
        private readonly DatabaseContext _context;

        public TicketsConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public long AddTickets(long? userId, Tickets tickets, TicketDetails ticketDetails,
            List<Attachments> attachments, List<AttachmentDetails> attachmentDetails)
        {
            using (SqlConnection con =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    var paramticket = new DynamicParameters();
                    paramticket.Add("@TrackingId", tickets.TrackingId);
                    paramticket.Add("@Name", tickets.Name);
                    paramticket.Add("@Email", tickets.Email);
                    paramticket.Add("@PriorityId", tickets.PriorityId);
                    paramticket.Add("@CategoryId", tickets.CategoryId);
                    paramticket.Add("@Contact", tickets.Contact);
                    paramticket.Add("@CreatedDate", tickets.CreatedDate);
                    paramticket.Add("@UserId", userId);
                    paramticket.Add("@StatusAssigned", tickets.StatusAssigned);
                    paramticket.Add("@InternalUserId", tickets.InternalUserId);
                    paramticket.Add("@TicketId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    var resultticket = con.Execute("Usp_InsertTicket", paramticket, transaction, 0,
                        CommandType.StoredProcedure);
                    var returnticketid = paramticket.Get<Int64>("@TicketId");


                    var paramticketDetails = new DynamicParameters();
                    paramticketDetails.Add("@Subject", ticketDetails.Subject);
                    paramticketDetails.Add("@Message", ticketDetails.Message);
                    paramticketDetails.Add("@UserId", userId);
                    paramticketDetails.Add("@TicketId", returnticketid);
                    var resultticketDetails = con.Execute("Usp_Insert_TicketDetails", paramticketDetails, transaction,
                        0, CommandType.StoredProcedure);

                    if (attachments != null)
                    {
                        for (var i = 0; i < attachments.Count; i++)
                        {
                            var objattachments = attachments[i];
                            var objattachmentsdetails = attachmentDetails[i];

                            var paramAttachments = new DynamicParameters();
                            paramAttachments.Add("@AttachmentName", objattachments.AttachmentName);
                            paramAttachments.Add("@AttachmentType", objattachments.AttachmentType);
                            paramAttachments.Add("@CreatedDate", DateTime.Now);
                            paramAttachments.Add("@UserId", userId);
                            paramAttachments.Add("@TicketId", returnticketid);
                            paramAttachments.Add("@AttachmentId", dbType: DbType.Int64,
                                direction: ParameterDirection.Output);
                            var resultAttachments = con.Execute("Usp_InsertAttachments", paramAttachments, transaction,
                                0, CommandType.StoredProcedure);
                            long attachmentId = paramAttachments.Get<Int64>("@AttachmentId");

                            var paramAttachmentsdetail = new DynamicParameters();
                            paramAttachmentsdetail.Add("@AttachmentBytes", objattachmentsdetails.AttachmentBytes);
                            paramAttachmentsdetail.Add("@AttachmentId", attachmentId);
                            paramAttachmentsdetail.Add("@UserId", userId);
                            paramAttachmentsdetail.Add("@TicketId", returnticketid);
                            var resultAttachmentsdetail = con.Execute("Usp_InsertAttachmentsDetails",
                                paramAttachmentsdetail, transaction, 0, CommandType.StoredProcedure);
                        }
                    }

                    var paramTicketDeleteLockStatus = new DynamicParameters();
                    paramTicketDeleteLockStatus.Add("@TicketId", returnticketid);
                    var resultTicketDeleteLockStatus = con.Execute("Usp_Insert_TicketDeleteLockStatus",
                        paramTicketDeleteLockStatus, transaction, 0, CommandType.StoredProcedure);

                    if (resultticket > 0 && resultticketDetails > 0 && resultTicketDeleteLockStatus > 0)
                    {
                        transaction.Commit();
                        return returnticketid;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }

        }

        public TicketsUserViewModel EditTicketsByTicketId(string trackingId)
        {

            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<TicketsUserViewModel>("Usp_Tickets_EditTicketsByTicketId", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public TicketReplyViewModel EditTicketsByTicketReplyId(string trackingId, long? ticketReplyId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    param.Add("@TicketReplyId", ticketReplyId);
                    return con.Query<TicketReplyViewModel>("Usp_Tickets_EditTicketsByTicketReplyId", param, null, false,
                        0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ViewTicketModel> GetAllTicketsbyUserId(string search, string searchin, int? statusId, long userId, int startIndex, int count, string sorting)
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
                query = query.Where(p => p.UserId == userId && p.StatusId == statusId);
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

        public int GetTicketsCount(long userId, string search, int? statusId, string searchin)
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
                    param.Add("@statusId", statusId);
                    return con.Query<int>("Usp_Tickets_GetTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool ChangeTicketPriority(ChangePriorityRequestModel changePriorityRequestModel)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@PriorityId", changePriorityRequestModel.PriorityId);
                    param.Add("@TicketId", changePriorityRequestModel.TicketId);
                    var result = con.Execute("Usp_ChangeTicketPriority", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeTicketStatus(ChangeStatusRequestModel changeStatusRequestModel)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@StatusId", changeStatusRequestModel.StatusId);
                    param.Add("@TicketId", changeStatusRequestModel.TicketId);
                    param.Add("@FirstResponseStatus", changeStatusRequestModel.FirstResponseStatus);
                    param.Add("@ResolutionStatus", changeStatusRequestModel.ResolutionStatus);
                    param.Add("@EveryResponseStatus", changeStatusRequestModel.EveryResponseStatus);
                    param.Add("@EscalationStatus", changeStatusRequestModel.EscalationStatus);
                    param.Add("@PriorityId", changeStatusRequestModel.PriorityId);
                    var result = con.Execute("Usp_ChangeTicketStatus", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeTicketCategory(ChangeCategoryRequestModel changeCategoryRequestModel)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", changeCategoryRequestModel.CategoryId);
                    param.Add("@TicketId", changeCategoryRequestModel.TicketId);
                    var result = con.Execute("Usp_ChangeTicketCategory", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateTickets(TicketsUserViewModel ticketsViewModel, List<Attachments> attachments,
            List<AttachmentDetails> attachmentDetails)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketsViewModel.TicketId);
                    param.Add("@Name", ticketsViewModel.Name);
                    param.Add("@Email", ticketsViewModel.Email);
                    param.Add("@PriorityId", ticketsViewModel.PriorityId);
                    param.Add("@CategoryId", ticketsViewModel.CategoryId);
                    param.Add("@Contact", ticketsViewModel.Contact);
                    param.Add("@Message", ticketsViewModel.Message);
                    param.Add("@Subject", ticketsViewModel.Subject);
                    var result = con.Execute("Usp_UpdateTicket", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        using (var context = new DatabaseContext())
                        {
                            if (attachments != null)
                            {

                                for (var i = 0; i < attachments.Count; i++)
                                {
                                    var objattachments = attachments[i];
                                    var objattachmentsdetails = attachmentDetails[i];

                                    objattachments.TicketId = ticketsViewModel.TicketId;
                                    objattachments.CreatedDate = DateTime.Now;
                                    context.Attachments.Add(objattachments);
                                    context.SaveChanges();

                                    objattachmentsdetails.TicketId = ticketsViewModel.TicketId;
                                    objattachmentsdetails.AttachmentId = objattachments.AttachmentId;
                                    context.AttachmentDetails.Add(objattachmentsdetails);
                                    context.SaveChanges();
                                }
                            }
                        }

                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int UpdateUserTickets(TicketsUserViewModel ticketsViewModel, List<Attachments> attachments,
          List<AttachmentDetails> attachmentDetails)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketsViewModel.TicketId);
                    param.Add("@Name", ticketsViewModel.Name);
                    param.Add("@Email", ticketsViewModel.Email);
                    param.Add("@PriorityId", ticketsViewModel.PriorityId);
                    param.Add("@Contact", ticketsViewModel.Contact);
                    param.Add("@Message", ticketsViewModel.Message);
                    param.Add("@Subject", ticketsViewModel.Subject);
                    var result = con.Execute("Usp_UpdateUserTicket", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        using (var context = new DatabaseContext())
                        {
                            if (attachments != null)
                            {

                                for (var i = 0; i < attachments.Count; i++)
                                {
                                    var objattachments = attachments[i];
                                    var objattachmentsdetails = attachmentDetails[i];

                                    objattachments.TicketId = ticketsViewModel.TicketId;
                                    objattachments.CreatedDate = DateTime.Now;
                                    context.Attachments.Add(objattachments);
                                    context.SaveChanges();

                                    objattachmentsdetails.TicketId = ticketsViewModel.TicketId;
                                    objattachmentsdetails.AttachmentId = objattachments.AttachmentId;
                                    context.AttachmentDetails.Add(objattachmentsdetails);
                                    context.SaveChanges();
                                }
                            }
                        }

                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateTicketReply(TicketReplyViewModel ticketsViewModel, List<ReplyAttachment> replyAttachment,
            List<ReplyAttachmentDetails> replyAttachmentDetails, long? ticketUser, long? systemUser)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketReplyId", ticketsViewModel.TicketReplyId);
                    param.Add("@Message", ticketsViewModel.Message);
                    param.Add("@Note", ticketsViewModel.Note);
                    var result = con.Execute("Usp_UpdateTicketReply", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (replyAttachment != null)
                    {
                        for (var i = 0; i < replyAttachment.Count; i++)
                        {

                            var paramReplyAttachment = new DynamicParameters();
                            paramReplyAttachment.Add("@AttachmentName", replyAttachment[i].AttachmentName);
                            paramReplyAttachment.Add("@AttachmentType", replyAttachment[i].AttachmentType);
                            paramReplyAttachment.Add("@TicketUser", ticketUser);
                            paramReplyAttachment.Add("@SystemUser", systemUser);
                            paramReplyAttachment.Add("@TicketReplyId", ticketsViewModel.TicketReplyId);
                            paramReplyAttachment.Add("@TicketId", ticketsViewModel.TicketId);
                            paramReplyAttachment.Add("@AttachmentBytes", replyAttachmentDetails[i].AttachmentBytes);
                            var resultAttachment = con.Execute("Usp_Insert_ReplyAttachment", paramReplyAttachment,
                                transaction, 0, CommandType.StoredProcedure);
                        }
                    }

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateAssignTickettoUser(long userId, long ticketId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_AssignTickettoUserbyUserId", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ViewTicketModel> GetAllAgentTicketsbyUserId(long userId, string search, string searchin,
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
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == false && t.ResolutionStatus == false && t.EveryResponseStatus == false)
                                                 && t.EscalationStatus == false
                                                 && t.UserId == userId);
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
                    else if (statussearch == 8 || statussearch == 9)
                    {

                    }
                    else
                    {
                        query = query
                            .Where(p => p.StatusId == 1 || p.StatusId == 3 || p.StatusId == 4 || p.StatusId == 5 ||
                                        p.StatusId == 6 || p.StatusId == 7).OrderByDescending(p => p.TicketUpdatedDate);
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

        public int DeleteTicket(long userId, long ticketId, short statusId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@StatusId", statusId);
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_DeleteTicket", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ViewTicketModel> GetAllDeleteTickets(string search, string searchin, int? prioritysearch,
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
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId
                        });




                    query = query.Where(t => t.StatusAssigned == true && t.DeleteStatus == true && t.IsActive == true);

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

                    if (statussearch != null)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }
                    else
                    {
                        query = query
                            .Where(p => p.StatusId == 1 || p.StatusId == 2 || p.StatusId == 3 || p.StatusId == 4 ||
                                        p.StatusId == 5 || p.StatusId == 6 || p.StatusId == 7)
                            .OrderByDescending(p => p.TicketUpdatedDate);
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

        public int GetDeleteTicketsAgentCount(long userId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<int>("Usp_Tickets_GetDeletedTicketsAgentCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int AssignTickettoAgent(long userId, long ticketId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_AssignTickettoAgent", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetTicketsAgentCount(long userId, string search, string searchin, int? prioritysearch,
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
                    return con.Query<int>("Usp_Tickets_GetTicketsAgentCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch)
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
                    return con.Query<int>("Usp_Tickets_GetAllTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> GetAllAdminTicketsbyList(string search, string searchin, int? prioritysearch,
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
                    else if (statussearch == 10)
                    {
                        query = query.Where(t => t.IsActive == true);
                    }
                    else
                    {
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == false || t.ResolutionStatus == false || t.EveryResponseStatus == false)
                                                 && t.EscalationStatus == false
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




        public int GetAllAgentAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch, int categoryId)
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
                    return con.Query<int>("Usp_Tickets_GetAllAgentAdminTicketsCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> GetAllAgentAdminTicketsbyList(string search, string searchin, int categoryId,
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
                        query = query.Where(t => t.IsActive == true
                                                 && t.DeleteStatus == false
                                                 && (t.FirstResponseStatus == false && t.ResolutionStatus == false && t.EveryResponseStatus == false)
                                                 && t.EscalationStatus == false
                                                 && t.CategoryId == categoryId
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

        public int LockTicket(long? ticketId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_LockTicket", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTicketLockStatus(long ticketId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    return con.Query<int>("Usp_GetTicketLockStatus", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UnLockTicket(long? ticketId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_UnLockTicket", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetDeleteTicketsAgentsAdminCount(long userId, string categoryId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<int>("Usp_Tickets_GetDeletedTicketsAgentsAdminCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewTicketModel> GetAllAgentsAdminDeleteTickets(string search, string searchin, int categoryId,
            int? prioritysearch,
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
                            DeleteStatus = x.deletelock.TicketDeleteStatus,
                            CategoryId = x.t6.t5.t4.t3.t2.ticketStatus.CategoryId,
                            IsActive = x.t6.t5.t4.t3.t2.ticketStatus.IsActive
                        });




                    query = query.Where(t =>
                        t.StatusAssigned == true && t.DeleteStatus == true && t.CategoryId == categoryId && t.IsActive == true);

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

                    if (statussearch != null)
                    {
                        query = query.Where(p => p.StatusId == statussearch);
                    }
                    else
                    {
                        query = query
                            .Where(p => p.StatusId == 1 || p.StatusId == 2 || p.StatusId == 3 || p.StatusId == 4 ||
                                        p.StatusId == 5 || p.StatusId == 6 || p.StatusId == 7)
                            .OrderByDescending(p => p.TicketUpdatedDate);
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


        public List<ViewTicketModel> GetAllDeleteTicketsbyAgent(long userId, string search, string searchin, int? prioritysearch, int? statussearch, int startIndex, int count, string sorting)
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
                                DeleteStatus = x.deletelock.TicketDeleteStatus
                            });

                    query = query.Where(p => p.StatusAssigned == true && p.StatusId == 7 && p.DeleteStatus == true);

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

                    if (statussearch != null)
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
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int RestoreTicket(long userId, long ticketId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                param.Add("@UserId", userId);
                var result = con.Execute("Usp_RestoreTicket", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return result;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }
            }
        }
    }
}