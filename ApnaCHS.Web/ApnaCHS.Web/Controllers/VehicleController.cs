using ApnaCHS.Common;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Vehicle")]
    public class VehicleController : BaseController
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController()
            : base(new UserService())
        {
            _vehicleService = new VehicleService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]VehicleViewModel viewmodel)
        {
            var model = Mapper.Map<Vehicle>(viewmodel);
            var result = await _vehicleService.Create(model);

            return Ok(Mapper.Map<VehicleViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _vehicleService.Read(id);
            return Ok(Mapper.Map<VehicleViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]VehicleViewModel viewmodel)
        {
            var model = Mapper.Map<Vehicle>(viewmodel);
            await _vehicleService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _vehicleService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]VehicleSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<VehicleSearchParams>(searchParams);
            var result = await _vehicleService.List(inParams);
            var viewmodels = new List<VehicleViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<VehicleViewModel>(item));
            }

            return Ok(viewmodels);
        }


        [HttpPost]
        [Route("Approve")]
        public async Task<IHttpActionResult> Approve([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;
            long societyId = viewmodel.societyId;

            var model = await _vehicleService.Approve(id, societyId, note, CurrentUser);
            return Ok(new { isApproved = model.IsApproved });
        }

        [HttpPost]
        [Route("BulkApprove")]
        public async Task<IHttpActionResult> BulkApprove([FromBody]dynamic viewmodel)
        {
            int[] ids = viewmodel.ids.ToObject<int[]>();
            string note = viewmodel.note;
            long societyId = viewmodel.societyId;

            var model = await _vehicleService.BulkApprove(ids, societyId, note, CurrentUser);
            return Ok(model.ToArray());
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IHttpActionResult> Reject([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            await _vehicleService.Reject(id, note, CurrentUser);
            return Ok();
        }
    }
}