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
    public class TicketsUserViewModel
    {
        public long TicketId { get; set; }
        [MaxLength(20)]
        public string TrackingId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [MaxLength(50)]
        [DisplayName("Email-Id")]
        [Required(ErrorMessage = "Please enter EmailID.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter valid Email Id ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Priority.")]
        [DisplayName("Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Mobile Number code should start with 6,7,8,9")]
        public string Contact { get; set; }

        public DateTime? CreatedDate { get; set; }
        public long? TicketDetailsId { get; set; }

        [Required(ErrorMessage = "Please Enter Subject.")]
        public string Subject { get; set; }

        [MaxLength(2000)]
        [AllowHtml]
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }
        public bool HasAttachment { get; set; }
        public List<Attachments> ListofAttachments { get; set; }
    }


    public class TicketsViewModel
    {
        public long? HiddenUserId { get; set; }

        public long TicketId { get; set; }
        [MaxLength(20)]
        public string TrackingId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Select Priority.")]
        [DisplayName("Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        public DateTime? CreatedDate { get; set; }
        public long? TicketDetailsId { get; set; }

        [Required(ErrorMessage = "Please Enter Subject.")]
        public string Subject { get; set; }

        [MaxLength(2000)]
        [AllowHtml]
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }
        public bool HasAttachment { get; set; }
        public List<Attachments> ListofAttachments { get; set; }
    }
}
