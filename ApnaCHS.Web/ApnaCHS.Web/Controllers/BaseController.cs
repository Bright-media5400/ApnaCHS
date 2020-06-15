using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ApnaCHS.Entities;
using ApnaCHS.Services;

namespace ApnaCHS.Web.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IUserService _userService = null;

        private ApplicationUser _currentUser;

        public BaseController(IUserService usersService)
        {
            this._userService = usersService;
        }
        
        public ApplicationUser CurrentUser
        {
            get
            {
                return _userService.FindByUserName(User.Identity.Name);
            }
            set
            {
                _currentUser = value;
            }
        }

    }
}