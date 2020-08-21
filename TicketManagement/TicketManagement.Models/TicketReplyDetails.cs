using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketReplyDetails")]
    public class TicketReplyDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketReplyDetailsId { get; set; }
        public string Message { get; set; }
        public long? TicketReplyId { get; set; }
    }
}
