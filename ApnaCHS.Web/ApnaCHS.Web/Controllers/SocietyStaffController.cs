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
    [RoutePrefix("api/SocietyStaff")]
    public class SocietyStaffController : BaseController
    {
        private readonly ISocietyStaffService _societyStaffService;

        public SocietyStaffController()
            : base(new UserService())
        {
            _societyStaffService = new SocietyStaffService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]SocietyStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyStaff>(viewmodel);
            var result = await _societyStaffService.Create(model);

            return Ok(Mapper.Map<SocietyStaffViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _societyStaffService.Read(id);
            return Ok(Mapper.Map<SocietyStaffViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]SocietyStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyStaff>(viewmodel);
            await _societyStaffService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateLastWorkingDay")]
        public async Task<IHttpActionResult> UpdateLastWorkingDay([FromBody]SocietyStaffViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyStaff>(viewmodel);
            await _societyStaffService.UpdateLastWorkingDay(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _societyStaffService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]SocietyStaffSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<SocietyStaffSearchParams>(searchParams);
            var result = await _societyStaffService.List(inParams);
            var viewmodels = new List<SocietyStaffViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<SocietyStaffViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}