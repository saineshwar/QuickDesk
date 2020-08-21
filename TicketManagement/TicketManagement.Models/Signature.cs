using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Signatures")]
    public class Signatures
    {
        [Key]
        public int SignatureId { get; set; }
        public string Signature { get; set; }
        public long UserId { get; set; }
    }
}
