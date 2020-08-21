using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Interface;
using TicketManagement.Models;

namespace TicketManagement.Concrete
{
    public class AttachmentsConcrete : IAttachments
    {

        private readonly DatabaseContext _context;
        public AttachmentsConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public Attachments GetAttachmentsByAttachmentId(long ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.Attachments
                                       where attachments.TicketId == ticketId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Attachments GetAttachments(long attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.Attachments
                                       where attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckAttachmentsExistbyTicketId(string trackingId)
        {
            try
            {
                var ticketId = (from ticket in _context.Tickets
                                where ticket.TrackingId == trackingId
                                select ticket.TicketId).FirstOrDefault();

                var attachmentsinfo = (from attachments in _context.Attachments
                                       where attachments.TicketId == ticketId
                                       select attachments.AttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AttachmentDetails> GetListAttachmentDetailsByAttachmentId(long? attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.AttachmentDetails
                                       where attachments.AttachmentId == attachmentId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReplyAttachment> GetListReplyAttachmentsByAttachmentId(long? ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.ReplyAttachment
                                       where attachments.TicketId == ticketId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReplyAttachmentDetails> GetListReplyAttachmentDetailsByAttachmentId(long? replyAttachmentId)
        {
            try
            {

                var attachmentsinfo = (from attachments in _context.ReplyAttachmentDetails
                                       where attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Attachments> GetListAttachmentsByAttachmentId(long? ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.Attachments
                                       where attachments.TicketId == ticketId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttachmentDetails GetAttachmentDetailsByAttachmentId(long? attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.AttachmentDetails
                                       where attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReplyAttachment GetReplyAttachmentsByAttachmentId(long? ticketReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.ReplyAttachment
                                       where attachments.TicketReplyId == ticketReplyId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReplyAttachmentDetails GetReplyAttachmentDetailsByAttachmentId(long? replyAttachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.ReplyAttachmentDetails
                                       where attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteAttachmentByAttachmentId(long? attachmentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@AttachmentId", attachmentId);
                    var result = con.Execute("Usp_DeleteAttachment", param, null, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteReplyAttachmentByAttachmentId(long? ticketId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    var result = con.Execute("Usp_DeleteReplyAttachment", param, null, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AttachmentsExistbyTicketId(long? ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _context.Attachments
                                       where attachments.TicketId == ticketId
                                       select attachments.AttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteReplyAttachmentByAttachmentIdAgent(long? replyAttachmentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@ReplyAttachmentId", replyAttachmentId);
                    var result = con.Execute("Usp_DeleteReplyAttachment_Agent", param, null, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return result;
                    }
                    else
                    {
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
