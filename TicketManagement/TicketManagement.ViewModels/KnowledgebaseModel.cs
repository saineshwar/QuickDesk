using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class KnowledgebaseModel
    {
        public long KnowledgebaseId { get; set; }
        public string Subject { get; set; }
        public string CategoryName { get; set; }
        public int? KnowledgebaseTypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool Status { get; set; }
        public string KnowledgebaseTypeName { get; set; }
        public int? CategoryId { get; set; }
    }


}
