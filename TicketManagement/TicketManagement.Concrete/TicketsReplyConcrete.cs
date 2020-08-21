using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class TicketsReplyConcrete : ITicketsReply
    {
        private readonly DatabaseContext _context;
        public TicketsReplyConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<ViewTicketReplyHistoryModel> ListofHistoryTicketReplies(string trackingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<ViewTicketReplyHistoryModel>("Usp_Tickets_HistoryTicketRepliesbytrackingId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReplyTicket(TicketReplyModel ticketReplyModel, List<ReplyAttachment> replyAttachment, List<ReplyAttachmentDetails> replyAttachmentDetails,
            long? fromuser, long? systemuser, int? status)
        {
        

            using (SqlConnection con =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    var paramTicketReply = new DynamicParameters();
                    paramTicketReply.Add("@CreatedDateDisplay", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    paramTicketReply.Add("@CreatedDate", DateTime.Now);
                    paramTicketReply.Add("@TicketUser", fromuser);
                    paramTicketReply.Add("@SystemUser", systemuser);
                    paramTicketReply.Add("@TicketId", ticketReplyModel.TicketId);
                    paramTicketReply.Add("@DeleteStatus", 0);
                    paramTicketReply.Add("@TicketReplyId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    var resultticketReply = con.Execute("Usp_Insert_TicketReply", paramTicketReply, transaction, 0,
                        CommandType.StoredProcedure);
                    long ticketReplyId = paramTicketReply.Get<Int64>("@TicketReplyId");


                    if (status != null)
                    {
                        var paramstatus = new DynamicParameters();
                        paramstatus.Add("@StatusId", status);
                        paramstatus.Add("@TicketId", ticketReplyModel.TicketId);
                        var resultstatus = con.Execute("Usp_UpdateTicketStatusbyTicketId", paramstatus, transaction, 0,
                            CommandType.StoredProcedure);
                    }

                    var paramTicketReplyDetails = new DynamicParameters();
                    paramTicketReplyDetails.Add("@TicketReplyId", ticketReplyId);
                    paramTicketReplyDetails.Add("@Message", ticketReplyModel.Message);
                    paramTicketReplyDetails.Add("@Note", ticketReplyModel.Note);
                    var resultTicketReplyDetails = con.Execute("Usp_Insert_TicketReplyDetails", paramTicketReplyDetails,
                        transaction, 0, CommandType.StoredProcedure);

                    if (replyAttachment.Count > 0)
                    {
                        for (var i = 0; i < replyAttachment.Count; i++)
                        {
                            var objattachments = replyAttachment[i];
                            var objattachmentsdetails = replyAttachmentDetails[i];

                            var paramReplyAttachment = new DynamicParameters();
                            paramReplyAttachment.Add("@AttachmentName", objattachments.AttachmentName);
                            paramReplyAttachment.Add("@AttachmentType", objattachments.AttachmentType);
                            paramReplyAttachment.Add("@TicketUser", fromuser);
                            paramReplyAttachment.Add("@SystemUser", systemuser);
                            paramReplyAttachment.Add("@TicketReplyId", ticketReplyId);
                            paramReplyAttachment.Add("@TicketId", ticketReplyModel.TicketId);
                            paramReplyAttachment.Add("@AttachmentBytes", objattachmentsdetails.AttachmentBytes);
                            var resultAttachment = con.Execute("Usp_Insert_ReplyAttachment", paramReplyAttachment,
                                transaction, 0, CommandType.StoredProcedure);
                        }
                    }

                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public void DeleteTicket(string trackingId, long? ticketreplyid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    con.Query<ViewTicketReplyHistoryModel>("Usp_Tickets_HistoryTicketRepliesbytrackingId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteTicketReply(long? ticketReplyId, long? ticketId, long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketReplyId", ticketReplyId);
                    param.Add("@TicketId", ticketId);
                    param.Add("@UserId", userId);
                    var result = con.Execute("Usp_DeleteTicketReply", param, transaction, 0, CommandType.StoredProcedure);

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


        public int RestoreTicketReply(long? ticketReplyId, long? ticketId, long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@TicketReplyId", ticketReplyId);
                    param.Add("@TicketId", ticketId);
                    param.Add("@UserId", userId);
                    var result = con.Execute("Usp_RestoreTicketReply", param, transaction, 0, CommandType.StoredProcedure);

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
    }
}
