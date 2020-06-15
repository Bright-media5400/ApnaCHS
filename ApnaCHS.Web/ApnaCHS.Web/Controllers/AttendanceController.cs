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
    [RoutePrefix("api/Attendance")]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController()
            : base(new UserService())
        {
            _attendanceService = new AttendanceService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]AttendanceViewModel viewmodel)
        {
            var model = Mapper.Map<Attendance>(viewmodel);
            var result = await _attendanceService.Create(model);

            return Ok(Mapper.Map<AttendanceViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _attendanceService.Read(id);
            return Ok(Mapper.Map<AttendanceViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]AttendanceViewModel viewmodel)
        {
            var model = Mapper.Map<Attendance>(viewmodel);
            await _attendanceService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _attendanceService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]AttendanceSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<AttendanceSearchParams>(searchParams);
            var result = await _attendanceService.List(inParams);
            var viewmodels = new List<AttendanceViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<AttendanceViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}