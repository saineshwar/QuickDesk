using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class UsermasterViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public bool? Status { get; set; }
        public string RoleName { get; set; }
        public int? RoleId { get; set; }
    }

    public class EscalatedUserViewModel
    {
        public string UserStage1 { get; set; }
        public string UserStage2 { get; set; }
        public DateTime? EscalationDate1 { get; set; }
        public DateTime? EscalationDate2 { get; set; }
    }
}
