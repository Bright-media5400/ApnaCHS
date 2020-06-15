using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using ApnaCHS.Common;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/ApplicationRole")]
    public class ApplicationRoleController : BaseController
    {
        private readonly IApplicationRoleService _applicationRoleService;

        public ApplicationRoleController()
            : base(new UserService())
        {
            _applicationRoleService = new ApplicationRoleService();
        }

        [HttpPost]
        [Route("List")]
        [Authorize(Roles = "Backend")]
        public async Task<IHttpActionResult> List()
        {
            var roles = await _applicationRoleService.AdminRoles();
            var viewmodels = new List<ApplicationRoleViewModel>();

            foreach (var role in roles)
            {
                var roleViewModel = Mapper.Map<ApplicationRoleViewModel>(role);
                viewmodels.Add(roleViewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Groups")]
        [Authorize(Roles = "Backend,Frontend")]
        public async Task<IHttpActionResult> Groups([FromBody]ApplicationRoleSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<ApplicationRoleSearchParams>(searchParams);
            var roles = await _applicationRoleService.Groups(inParams);
            var viewmodels = new List<ApplicationRoleViewModel>();

            foreach (var role in roles)
            {
                var roleViewModel = Mapper.Map<ApplicationRoleViewModel>(role);
                viewmodels.Add(roleViewModel);
            }

            return Ok(viewmodels);
        }

        //[HttpPost]
        //[Route("GetApplicationRoles")]
        //public async Task<IHttpActionResult> GetApplicationRoles()
        //{
        //    var roles = await _applicationRoleService.ApplicationRoleListForDisplay();
        //    var viewmodels = new List<ApplicationRoleViewModel>();

        //    foreach (var role in roles)
        //    {
        //        viewmodels.Add(Mapper.Map<ApplicationRoleViewModel>(role));
        //    }

        //    return Ok(viewmodels);
        //}

        [HttpPost]
        [Route("AddUpdateRole")]
        [Authorize(Roles = "Backend")]
        public async Task<IHttpActionResult> AddUpdateRole([FromBody]ApplicationRoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Model not valid");
            }
            var roleModel = Mapper.Map<ApplicationRole>(role);
            roleModel.IsBack = true;

            if (role.id > 0)
            {
                await _applicationRoleService.UpdateApplicationRole(roleModel);
            }
            else
            {
                await _applicationRoleService.NewApplicationRole(roleModel);
            }

            return Ok(Mapper.Map<ApplicationRoleViewModel>(roleModel));
        }


        [HttpPost]
        [Route("AddUpdateGroup")]
        [Authorize(Roles = "Backend,Frontend")]
        public async Task<IHttpActionResult> AddUpdateGroup([FromBody]ApplicationRoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Model not valid");
            }

            var roleModel = Mapper.Map<ApplicationRole>(role);
            roleModel.IsBack = false;

            if (role.id > 0)
            {
                await _applicationRoleService.UpdateApplicationRole(roleModel);
            }
            else
            {
                await _applicationRoleService.NewApplicationRole(roleModel);
            }

            return Ok(Mapper.Map<ApplicationRoleViewModel>(roleModel));
        }

        [HttpPost]
        [Route("GetApplicationRoleDetail")]
        public async Task<IHttpActionResult> GetApplicationRoleDetail([FromBody]long id)
        {
            var role = await _applicationRoleService.GetApplicationRole(id);
            if (role != null)
                return Ok(Mapper.Map<ApplicationRoleViewModel>(role));
            throw new Exception("Person not found");
        }

        [HttpPost]
        [Route("GetApplicationRoleDetailbyName")]
        public async Task<IHttpActionResult> GetApplicationRoleDetailbyName([FromBody]string name)
        {
            var role = await _applicationRoleService.GetApplicationRole(name);
            if (role != null)
                return Ok(Mapper.Map<ApplicationRoleViewModel>(role));
            throw new Exception("Person not found");
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody]long id)
        {
            await _applicationRoleService.Delete(id);
            return Ok();
        }

    }
}
