using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class UserResponseViewModel
    {
        public string Username { get; set; }

        public long UserId { get; set; }
    }

    public class ProfileRequest
    {
        public string ProfileId { get; set; }
    }
}
