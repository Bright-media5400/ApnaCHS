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
    [RoutePrefix("api/FlatOwnerFamily")]
    public class FlatOwnerFamilyController : BaseController
    {
        private readonly IFlatOwnerFamilyService _flatOwnerFamilyService;

        public FlatOwnerFamilyController()
            : base(new UserService())
        {
            _flatOwnerFamilyService = new FlatOwnerFamilyService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]FlatOwnerFamilyViewModel viewmodel)
        {
            var model = Mapper.Map<FlatOwnerFamily>(viewmodel);
            var result = await _flatOwnerFamilyService.Create(model);

            return Ok(Mapper.Map<FlatOwnerFamilyViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _flatOwnerFamilyService.Read(id);
            return Ok(Mapper.Map<FlatOwnerFamilyViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]FlatOwnerFamilyViewModel viewmodel)
        {
            var model = Mapper.Map<FlatOwnerFamily>(viewmodel);
            await _flatOwnerFamilyService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _flatOwnerFamilyService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]FlatOwnerFamilySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FlatOwnerFamilySearchParams>(searchParams);
            var result = await _flatOwnerFamilyService.List(inParams);
            var viewmodels = new List<FlatOwnerFamilyViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FlatOwnerFamilyViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Report")]
        public async Task<IHttpActionResult> Report([FromBody]ReportFamilySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<ReportFamilySearchParams>(searchParams);
            var result = await _flatOwnerFamilyService.Report(inParams);
            var viewmodels = new List<ReportFamilyResultViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ReportFamilyResultViewModel>(item));
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

            await _flatOwnerFamilyService.Approve(id, societyId, null, note, CurrentUser);
            return Ok();
        }

        [HttpPost]
        [Route("BulkApprove")]
        public async Task<IHttpActionResult> BulkApprove([FromBody]dynamic viewmodel)
        {
            int[] ids = viewmodel.ids.ToObject<int[]>();
            string note = viewmodel.note;
            long societyId = viewmodel.societyId;

            var model = await _flatOwnerFamilyService.BulkApprove(ids, societyId, note, CurrentUser);
            return Ok(model.ToArray());
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IHttpActionResult> Reject([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            await _flatOwnerFamilyService.Reject(id, note, CurrentUser);
            return Ok();
        }

    }
}