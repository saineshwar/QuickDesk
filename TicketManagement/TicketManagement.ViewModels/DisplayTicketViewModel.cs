using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class DisplayTicketViewModel
    {
        public long TicketId { get; set; }
        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [DisplayName("Priority")]
        public string PriorityName { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public string Contact { get; set; }
        public string CreatedDate { get; set; }
        public long? UserId { get; set; }
        public bool StatusAssigned { get; set; }
        public long TicketDetailsId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        [DisplayName("Change Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Change Category")]
        public short? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        [DisplayName("Change Status")]
        public short? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }
        public TicketReplyModel TicketReply { get; set; }
        public ViewTicketReplyMainModel ViewMainModel { get; set; }
        public List<Attachments> ListofAttachments { get; set; }
        public int TicketLockStatus { get; set; }
        public string RoleName { get; set; }
        public bool? DeleteStatus { get; set; }
        public bool EscalationStatus { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool ResolutionStatus { get; set; }
        public bool EveryResponseStatus { get; set; }
        public EscalatedUserViewModel EscalatedUser { get; set; }
        public short EscalationStage { get; set; }
    }
}
