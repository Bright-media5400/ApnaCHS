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
    [RoutePrefix("api/FlatOwner")]
    public class FlatOwnerController : BaseController
    {
        private readonly IFlatOwnerService _flatOwnerService;

        public FlatOwnerController()
            : base(new UserService())
        {
            _flatOwnerService = new FlatOwnerService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]FlatOwnerViewModel viewmodel)
        {
            var model = Mapper.Map<FlatOwner>(viewmodel);
            var result = await _flatOwnerService.Create(model);

            return Ok(Mapper.Map<FlatOwnerViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _flatOwnerService.Read(id);
            return Ok(Mapper.Map<FlatOwnerViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]FlatOwnerViewModel flatowner)
        {
            var model = Mapper.Map<FlatOwner>(flatowner);
            await _flatOwnerService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateTillDate")]
        public async Task<IHttpActionResult> UpdateTillDate([FromBody]MapFlatToFlatOwnerViewModel viewmodel)
        {
            var model = Mapper.Map<MapFlatToFlatOwner>(viewmodel);
            await _flatOwnerService.UpdateTillDate(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateSinceDate")]
        public async Task<IHttpActionResult> UpdateSinceDate([FromBody]MapFlatToFlatOwnerViewModel viewmodel)
        {
            var model = Mapper.Map<MapFlatToFlatOwner>(viewmodel);
            await _flatOwnerService.UpdateSinceDate(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody]int id)
        {
            await _flatOwnerService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]FlatOwnerSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FlatOwnerSearchParams>(searchParams);
            var result = await _flatOwnerService.List(inParams);
            var viewmodels = new List<FlatOwnerViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FlatOwnerViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<IHttpActionResult> Approve([FromBody]dynamic viewmodel)
        {
            long flatOwner = viewmodel.flatOwner;
            long societyId = viewmodel.societyId;
            string note = viewmodel.note;

            await _flatOwnerService.Approve(flatOwner, societyId, null, note, CurrentUser);
            return Ok();
        }

        [HttpPost]
        [Route("BulkApprove")]
        public async Task<IHttpActionResult> BulkApprove([FromBody]dynamic viewmodel)
        {
            int[] ids = viewmodel.ids.ToObject<int[]>();
            string note = viewmodel.note;
            long societyId = viewmodel.societyId;

            var model = await _flatOwnerService.BulkApprove(ids, societyId, note, CurrentUser);

            var viewmodels = new List<ApprovalReplyViewModel>();

            foreach (var item in model)
            {
                viewmodels.Add(Mapper.Map<ApprovalReplyViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IHttpActionResult> Reject([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            await _flatOwnerService.Reject(id, note, CurrentUser);
            return Ok();
        }


        [HttpPost]
        [Route("UploadFlatOwners")]
        public async Task<IHttpActionResult> UploadFlatOwners([FromBody]dynamic viewmodel)
        {
            var flatowners = viewmodel.flatowners.ToObject<List<UploadFlatOwnerViewModel>>();

            var models = new List<UploadFlatOwner>();
            foreach (var item in flatowners)
            {
                models.Add(Mapper.Map<UploadFlatOwner>(item));
            }


            var result = await _flatOwnerService.UploadFlatOwners(models, 1);

            var viewmodels = new List<UploadFlatOwnerViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<UploadFlatOwnerViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UploadTenants")]
        public async Task<IHttpActionResult> UploadTenants([FromBody]dynamic viewmodel)
        {
            var flatowners = viewmodel.flatowners.ToObject<List<UploadFlatOwnerViewModel>>();

            var models = new List<UploadFlatOwner>();
            foreach (var item in flatowners)
            {
                models.Add(Mapper.Map<UploadFlatOwner>(item));
            }


            var result = await _flatOwnerService.UploadFlatOwners(models, 2);

            var viewmodels = new List<UploadFlatOwnerViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<UploadFlatOwnerViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("ExportFlatOwners")]
        public async Task<IHttpActionResult> ExportFlatOwners([FromBody]ReportFlatOwnersTenantsDetailSearchParamsViewModel searchParams)
        {
            var inParams = Mapper.Map<ReportFlatOwnersTenantsDetailSearchParams>(searchParams);
            var result = await _flatOwnerService.ExportFlatOwners(inParams);
            var viewmodels = new List<ReportFlatOwnersTenantsDetailViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ReportFlatOwnersTenantsDetailViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UploadFlatOwnersFamily")]
        public async Task<IHttpActionResult> UploadFlatOwnersFamily([FromBody]dynamic viewmodel)
        {
            var families = viewmodel.families.ToObject<List<ReportFlatOwnersTenantsDetailViewModel>>();

            var models = new List<ReportFlatOwnersTenantsDetail>();
            foreach (var item in families)
            {
                models.Add(Mapper.Map<ReportFlatOwnersTenantsDetail>(item));
            }


            var result = await _flatOwnerService.UploadFlatOwnersFamily(models);

            var viewmodels = new List<ReportFlatOwnersTenantsDetailViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ReportFlatOwnersTenantsDetailViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("ExportFlatOwnersVehicles")]
        public async Task<IHttpActionResult> ExportFlatOwnersVehicles([FromBody]ReportFlatOwnersTenantsDetailSearchParamsViewModel searchParams)
        {
            var inParams = Mapper.Map<ReportFlatOwnersTenantsDetailSearchParams>(searchParams);
            var result = await _flatOwnerService.ExportFlatOwnersVehicles(inParams);
            var viewmodels = new List<ReportFlatOwnersVehicleDetailViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ReportFlatOwnersVehicleDetailViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UploadFlatOwnersVehicle")]
        public async Task<IHttpActionResult> UploadFlatOwnersVehicle([FromBody]dynamic viewmodel)
        {
            var families = viewmodel.vehicles.ToObject<List<ReportFlatOwnersVehicleDetailViewModel>>();

            var models = new List<ReportFlatOwnersVehicleDetail>();
            foreach (var item in families)
            {
                models.Add(Mapper.Map<ReportFlatOwnersVehicleDetail>(item));
            }


            var result = await _flatOwnerService.UploadFlatOwnersVehicle(models);

            var viewmodels = new List<ReportFlatOwnersVehicleDetailViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ReportFlatOwnersVehicleDetailViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}