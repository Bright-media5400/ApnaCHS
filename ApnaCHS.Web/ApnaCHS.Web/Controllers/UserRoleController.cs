using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using ApnaCHS.Entities;

namespace ApnaCHS.Web.Controllers
{
     [Authorize]
    [RoutePrefix("api/UserRole")]
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController()
            : base(new UserService())
        {
            _userRoleService = new UserRoleService();
        }

        [HttpPost]
        [Route("GetUserRoles")]        
        public async Task<IHttpActionResult> GetUserRoles([FromBody]long id)
        {
            var roles = await _userRoleService.ApplicationRoleListForDisplay((int)id);
            var viewmodels = new List<ApplicationUserRoleViewModel>();

            foreach (var role in roles)
            {
                viewmodels.Add(Mapper.Map<ApplicationUserRoleViewModel>(role));
            }

            return Ok(viewmodels);
        }

        //[HttpPost]
        //[Route("GetUsers")]
        //public async Task<IHttpActionResult> GetUsers()
        //{
        //    var users = await _userService.UserListForDisplay();
        //    var viewmodels = new List<ApplicationUserViewModel>();

        //    foreach (var user in users)
        //    {
        //        viewmodels.Add(Mapper.Map<ApplicationUserViewModel>(user));
        //    }

        //    return Ok(viewmodels);
        //}

        [HttpPost]
        [Route("AddUserRoles")]
        public async Task<IHttpActionResult> AddUserRoles(List<ApplicationRoleViewModel> userRoles)
        {
            var userRoleList = new List<ApplicationUserRole>();
            foreach (var role in userRoles)
            {
                userRoleList.Add(Mapper.Map<ApplicationUserRole>(role));
            }
            await _userRoleService.AddUserRoles(userRoleList);
            return Ok(userRoles);
        }
    }
}
