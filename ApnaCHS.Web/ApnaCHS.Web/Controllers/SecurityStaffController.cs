using ApnaCHS.Common;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Controllers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ApnaCHS.Web.Models;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/SecurityStaff")]
    public class SecurityStaffController : BaseController
    {
        private readonly ISecurityStaffService _securityStaffService;

        public SecurityStaffController()
            : base(new UserService())
        {
            _securityStaffService = new SecurityStaffService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]SecurityStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SecurityStaff>(viewmodel);
            var result = await _securityStaffService.Create(model);

            return Ok(Mapper.Map<SecurityStaffViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _securityStaffService.Read(id);
            return Ok(Mapper.Map<SecurityStaffViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]SecurityStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SecurityStaff>(viewmodel);
            await _securityStaffService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateLastWorkingDay")]
        public async Task<IHttpActionResult> UpdateLastWorkingDay([FromBody]SecurityStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SecurityStaff>(viewmodel);
            await _securityStaffService.UpdateLastWorkingDay(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _securityStaffService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]SecurityStaffSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<SecurityStaffSearchParams>(searchParams);
            var result = await _securityStaffService.List(inParams);
            var viewmodels = new List<SecurityStaffViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<SecurityStaffViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}