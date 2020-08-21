using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("ResetPasswordVerification")]
    public class ResetPasswordVerification
    {
        [Key]
        public long ResetTokenId { get; set; }
        public string GeneratedToken { get; set; }
        public DateTime? GeneratedDate { get; set; }
        public bool VerificationStatus { get; set; }
        public bool Status { get; set; }
        public long UserId { get; set; }
        public DateTime? VerificationDate { get; set; }
    }
}
