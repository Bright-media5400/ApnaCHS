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

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Facility")]
    public class FacilityController : BaseController
    {
        private readonly IFacilityService _facilityService;

        public FacilityController()
            : base(new UserService())
        {
            _facilityService = new FacilityService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]dynamic inparams)
        {
            long? societyId = inparams.societyId;
            FacilityViewModel[] facilities = inparams.facilities.ToObject<FacilityViewModel[]>();

            var models = new List<Facility>();
            foreach (var item in facilities)
            {
                models.Add(Mapper.Map<Facility>(item));
            }

            var result = await _facilityService.Create(models.ToArray(), societyId);
            return Ok();
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _facilityService.Read(id);
            return Ok(Mapper.Map<FacilityViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]FacilityViewModel viewmodel)
        {
            var model = Mapper.Map<Facility>(viewmodel);
            await _facilityService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _facilityService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]FacilitySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FacilitySearchParams>(searchParams);
            var result = await _facilityService.List(inParams);
            
            var viewmodels = new List<FacilityTrimViewModel>();
            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FacilityTrimViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("SocietyWiseList")]
        public async Task<IHttpActionResult> SocietyWiseList([FromBody]FacilitySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FacilitySearchParams>(searchParams);
            var result = await _facilityService.SocietyWiseList(inParams);

            var viewmodels = new List<FacilityTrimViewModel>();
            if (result == null) { return Ok(viewmodels); }

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FacilityTrimViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("LoadFlats")]
        public async Task<IHttpActionResult> LoadFlats([FromBody]FacilitySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FacilitySearchParams>(searchParams);
            var result = await _facilityService.LoadFlats(inParams);
            var viewmodels = new List<FacilityForFlatViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FacilityForFlatViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("FlatCount")]
        public async Task<IHttpActionResult> FlatCount([FromBody] int id)
        {
            var result = await _facilityService.FlatCount(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("LoadParkings")]
        public async Task<IHttpActionResult> LoadParkings([FromBody]FacilitySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FacilitySearchParams>(searchParams);
            var result = await _facilityService.LoadParkings(inParams);
            var viewmodels = new List<FacilityForFlatParkingsViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FacilityForFlatParkingsViewModel>(item));
            }

            return Ok(viewmodels);
        }


        [HttpPost]
        [Route("LinkSocieties")]
        public async Task<IHttpActionResult> LinkSocieties([FromBody]dynamic inparams)
        {
            long[] linkSocieties = inparams.linkSocieties.ToObject<long[]>();
            long[] unlinkSocieties = inparams.unlinkSocieties.ToObject<long[]>();
            long facilityId = inparams.facilityId;

            await _facilityService.LinkSocieties(linkSocieties, unlinkSocieties, facilityId);
            return Ok();
        }

    }
}