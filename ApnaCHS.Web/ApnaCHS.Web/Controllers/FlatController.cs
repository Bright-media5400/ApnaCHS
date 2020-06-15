using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ApnaCHS.Common;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Controllers;
using ApnaCHS.Web.Models;
using Newtonsoft.Json;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Flat")]
    public class FlatController : BaseController
    {
        private readonly IFlatService _flatService;

        public FlatController()
            : base(new UserService())
        {
            _flatService = new FlatService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]dynamic viewmodel)
        {
            string flat = viewmodel.flat;
            int? count = viewmodel.count;
            var flatViewModel = JsonConvert.DeserializeObject<FlatViewModel>(flat);

            var model = Mapper.Map<Flat>(flatViewModel);
            var result = await _flatService.Create(model, count);

            return Ok(Mapper.Map<FlatViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _flatService.Read(id);
            return Ok(Mapper.Map<FlatViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]FlatViewModel viewmodel)
        {
            var model = Mapper.Map<Flat>(viewmodel);
            await _flatService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _flatService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]FlatSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FlatSearchParams>(searchParams);
            var result = await _flatService.List(inParams);
            var viewmodels = new List<FlatViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FlatViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Report")]
        public async Task<IHttpActionResult> Report([FromBody]FlatSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FlatSearchParams>(searchParams);
            var result = await _flatService.Report(inParams);
            var viewmodels = new List<FlatReportResultViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FlatReportResultViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("ExportFloors")]
        public async Task<IHttpActionResult> ExportFloors([FromBody]ReportFlatOwnersTenantsDetailSearchParamsViewModel searchParams)
        {
            var result = await _flatService.ExportFloors(searchParams.societyId.Value);
            var viewmodels = new List<UploadFlatViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<UploadFlatViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UploadFlats")]
        public async Task<IHttpActionResult> UploadFlats([FromBody]dynamic viewmodel)
        {
            var flats = viewmodel.flats.ToObject<List<UploadFlatViewModel>>();

            var models = new List<UploadFlat>();
            foreach (var item in flats)
            {
                models.Add(Mapper.Map<UploadFlat>(item));
            }


            var result = await _flatService.UploadFlats(models);

            var viewmodels = new List<UploadFlatViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<UploadFlatViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<IHttpActionResult> Approve([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            var model = await _flatService.Approve(id, note, CurrentUser);
            return Ok(new { isApproved = model.IsApproved });
        }

        [HttpPost]
        [Route("BulkApprove")]
        public async Task<IHttpActionResult> BulkApprove([FromBody]dynamic viewmodel)
        {
            int[] ids = viewmodel.ids.ToObject<int[]>();
            string note = viewmodel.note;

            var model = await _flatService.BulkApprove(ids, note, CurrentUser);
            return Ok(model.ToArray());
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IHttpActionResult> Reject([FromBody]dynamic viewmodel)
        {
            int id = viewmodel.id;
            string note = viewmodel.note;

            await _flatService.Reject(id, note, CurrentUser);
            return Ok();
        }

    }
}