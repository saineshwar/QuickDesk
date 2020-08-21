using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("ProfileImageStatus")]
    public class ProfileImageStatus
    {
        [Key]
        public long ProfileImageStatusId { get; set; }
        public bool Isuploaded { get; set; }
        public long? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ProfileImageId { get; set; }
        
    }
}
