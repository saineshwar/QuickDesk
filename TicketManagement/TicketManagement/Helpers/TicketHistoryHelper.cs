using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketManagement.CommonData;
using TicketManagement.Concrete;
using TicketManagement.Interface;

namespace TicketManagement.Helpers
{
    public class TicketHistoryHelper
    {
        private readonly DatabaseContext _context;
        public TicketHistoryHelper()
        {
            _context = new DatabaseContext();
        }

        public string CreateMessage(short? priorityId, short? categoryId)
        {
            string message = string.Empty;
            string priorityName = GetPriorityNameBypriorityId(priorityId);
            string status = GetStatusBystatusId(Convert.ToInt16(StatusMain.Status.Open));
            string categoryName = GetCategoryNameBycategoryId(categoryId);

            if (priorityId != null && categoryId != null)
            {
                message = $"Created a new ticket. Set status as {status}, priority as {priorityName}, category as {categoryName}";
            }

            return message;
        }

        public string StatusMessage(short? statusId)
        {
            string message = string.Empty;
            string status = GetStatusBystatusId(statusId);

            // Open | Resolved | InProgress | OnHold | Recently |Edited |Replied
            if (statusId != null &&
                   statusId == Convert.ToInt16(StatusMain.Status.Open)
                || statusId == Convert.ToInt16(StatusMain.Status.Resolved) 
                || statusId == Convert.ToInt16(StatusMain.Status.InProgress)
                || statusId == Convert.ToInt16(StatusMain.Status.OnHold)
                || statusId == Convert.ToInt16(StatusMain.Status.RecentlyEdited)
                || statusId == Convert.ToInt16(StatusMain.Status.Replied)
            )
            {
                message = $"Set status as {status}";
            }

            // Deleted
            if (statusId != null && statusId == Convert.ToInt16(StatusMain.Status.Deleted))
            {
                message = $"Deleted ticket";
            }

            // Closed
            if (statusId != null && statusId == Convert.ToInt16(StatusMain.Status.Closed))
            {
                message = $"Closed ticket";
            }

            return message;
        }

        public string PriorityMessage(short? priorityId)
        {
            string message = string.Empty;
            string priorityName = GetPriorityNameBypriorityId(priorityId);

            // Deleted
            if (priorityId != null)
            {
                message = $"Set priority as {priorityName}";
            }

            return message;
        }

        public string CategoryMessage(short? categoryId)
        {
            string message = string.Empty;
            string categoryName = GetCategoryNameBycategoryId(categoryId);

            // Deleted
            if (categoryId != null)
            {
                message = $"Set category as {categoryName}";
            }

            return message;
        }

        public string ReplyMessage(short? statusId)
        {
            string message = string.Empty;
            string status = GetStatusBystatusId(statusId);

            if (status != null)
            {
                message = $"Replied,a few seconds ago. And Set status as {status}";
            }

            return message;
        }

        public string DeleteTicketReplyMessage()
        {
            var message = $"Deleted Ticket Reply";
            return message;
        }

        public string EditTicket()
        {
            var message = "Edited Ticket";
            return message;
        }

        public string EditReplyTicket()
        {
            var message = "Edited Ticket Reply";
            return message;
        }

        public string DeleteTicketAttachment()
        {
            var message = "Deleted Ticket Attachment";
            return message;
        }

        public string DeleteTicketReplyAttachment()
        {
            var message = "Deleted Ticket Reply Attachment";
            return message;
        }

        public string TicketDelete()
        {
            var message = "Ticket Deleted";
            return message;
        }

        public string TicketRestore()
        {
            var message = "Ticket Restored";
            return message;
        }

        public string AssignTickettoUser()
        {
            var message = "Manually Assign Ticket";
            return message;
        }


        private string GetPriorityNameBypriorityId(short? priorityId)
        {
            var priorityList = (from priority in _context.Priority
                                where priority.PriorityId == priorityId
                                select priority.PriorityName).FirstOrDefault();
            return priorityList;
        }

        private string GetStatusBystatusId(short? statusId)
        {
            var priorityList = (from status in _context.Status
                                where status.StatusId == statusId
                                select status.StatusText).FirstOrDefault();
            return priorityList;
        }

        private string GetCategoryNameBycategoryId(short? categoryId)
        {
            var priorityList = (from category in _context.Category
                                where category.CategoryId == categoryId
                                select category.CategoryName).FirstOrDefault();
            return priorityList;
        }
    }
}