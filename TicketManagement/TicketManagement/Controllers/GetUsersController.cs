using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketManagement.CommonData;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    public class GetUsersController : ApiController
    {
        private readonly IUserMaster _userMaster;
        public GetUsersController(IUserMaster userMaster)
        {
            _userMaster = userMaster;
        }

        [HttpGet]
        // POST: api/GetUsers
        public List<UserResponseViewModel> Post(string usernames)
        {
            return _userMaster.GetAutoCompleteUserName(usernames, Convert.ToInt32(StatusMain.Roles.Agent));
        } 

    }
}
