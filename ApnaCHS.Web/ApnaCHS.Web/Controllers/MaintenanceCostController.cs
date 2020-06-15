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
    [RoutePrefix("api/MaintenanceCost")]
    public class MaintenanceCostController : BaseController
    {
        private readonly IMaintenanceCostService _maintenanceCostService;

        public MaintenanceCostController()
            : base(new UserService())
        {
            _maintenanceCostService = new MaintenanceCostService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]MaintenanceCostViewModel viewmodel)
        {
            var model = Mapper.Map<MaintenanceCost>(viewmodel);
            var result = await _maintenanceCostService.Create(model);

            return Ok(Mapper.Map<MaintenanceCostViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _maintenanceCostService.Read(id);
            return Ok(Mapper.Map<MaintenanceCostViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]MaintenanceCostViewModel viewmodel)
        {
            var model = Mapper.Map<MaintenanceCost>(viewmodel);
            await _maintenanceCostService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateEndDate")]
        public async Task<IHttpActionResult> UpdateEndDate([FromBody]MaintenanceCostViewModel viewmodel)
        {
            var model = Mapper.Map<MaintenanceCost>(viewmodel);
            await _maintenanceCostService.UpdateEndDate(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _maintenanceCostService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]MaintenanceCostSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<MaintenanceCostSearchParams>(searchParams);
            var result = await _maintenanceCostService.List(inParams);

            var viewmodels = new List<MaintenanceCostViewModel>();
            if (result == null) { return Ok(viewmodels); }

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<MaintenanceCostViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<IHttpActionResult> Approve([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            var model = await _maintenanceCostService.Approve(id, note, CurrentUser);
            return Ok(new { isApproved = model.IsApproved });
        }

        [HttpPost]
        [Route("BulkApprove")]
        public async Task<IHttpActionResult> BulkApprove([FromBody]dynamic viewmodel)
        {
            int []ids = viewmodel.ids.ToObject<int[]>();
            string note = viewmodel.note;

            var model = await _maintenanceCostService.BulkApprove(ids, note, CurrentUser);
            return Ok(model.ToArray());
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IHttpActionResult> Reject([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            await _maintenanceCostService.Reject(id, note, CurrentUser);
            return Ok();
        }

        [HttpPost]
        [Route("AssignedFlats")]
        public async Task<IHttpActionResult> AssignedFlats([FromBody]dynamic viewmodel)
        {
            int mcid = viewmodel.mcid;
            Flat[] assignedFlats = viewmodel.assignedFlats.ToObject<Flat[]>();
            Flat[] unassignedFlats = viewmodel.unassignedFlats.ToObject<Flat[]>();

            await _maintenanceCostService.AssignedFlats(mcid, assignedFlats, unassignedFlats);
            return Ok();
        }

        [HttpPost]
        [Route("Restore")]
        public async Task<IHttpActionResult> Restore([FromBody] int id)
        {
            await _maintenanceCostService.Restore(id);
            return Ok();
        }
    }
}