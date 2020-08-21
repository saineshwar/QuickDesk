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
    public class KnowledgebaseViewModel
    {
        public long? KnowledgebaseId { get; set; }

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Please Select Type")]
        public int? KnowledgebaseTypeId { get; set; }

        public List<SelectListItem> ListofKnowledgebaseType { get; set; }

        [DisplayName("Contents")]
        [Required(ErrorMessage = "Please Enter Contents")]
        [MaxLength(2000)]
        [AllowHtml]
        public string Contents { get; set; }

        [MaxLength(500)]
        [DisplayName("Keywords")]
        [Required(ErrorMessage = "Please Enter Keywords")]
        public string Keywords { get; set; }
        public bool Status { get; set; }


    }

    public class EditKnowledgebaseViewModel
    {
        public long? KnowledgebaseId { get; set; }

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Please Select Type")]
        public int? KnowledgebaseTypeId { get; set; }

        public List<SelectListItem> ListofKnowledgebaseType { get; set; }

        [DisplayName("Contents")]
        [Required(ErrorMessage = "Please Enter Contents")]
        [MaxLength(2000)]
        [AllowHtml]
        public string Contents { get; set; }

        [MaxLength(500)]
        [DisplayName("Keywords")]
        [Required(ErrorMessage = "Please Enter Keywords")]
        public string Keywords { get; set; }
        public bool Status { get; set; }
        public List<KnowledgebaseAttachments> ListofAttachments { get; set; }
    }

    public class KnowledgebaseAttachmentsDownload
    {
        public string AttachmentsName { get; set; }
        public string AttachmentsType { get; set; }
        public byte[] AttachmentBytes { get; set; }
    }

    public class RequestDeleteAttachment
    {
        public long? KnowledgebaseAttachmentsId { get; set; }
    }

    public class KnowledgebaseArticleViewModel
    {
        public long? KnowledgebaseId { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Type")]
        public string KnowledgebaseTypeName { get; set; }

        [DisplayName("Contents")]
        public string Contents { get; set; }
        [DisplayName("Keywords")]
        public string Keywords { get; set; }
        public bool Status { get; set; }
        public List<KnowledgebaseAttachments> ListofAttachments { get; set; }
    }

    public class KnowledgeSearch
    {
        public string Subject { get; set; }
        public long? KnowledgebaseId { get; set; }
        public string CategoryName { get; set; }
    }

    public class AllKnowledgebase
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int KnowledgebaseCount { get; set; }
        public string CategoryDescription { get; set; }     
    }
}
