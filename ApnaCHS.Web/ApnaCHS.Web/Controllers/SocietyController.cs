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
    [RoutePrefix("api/Society")]
    public class SocietyController : BaseController
    {
        private readonly ISocietyService _societyService;

        public SocietyController()
            : base(new UserService())
        {
            _societyService = new SocietyService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]SocietyViewModel viewmodel)
        {
            var model = Mapper.Map<Society>(viewmodel);
            var result = await _societyService.Create(model, CurrentUser);

            return Ok(Mapper.Map<SocietyViewModel>(result));
        }

        [Authorize(Roles = "Backend,Frontend")]
        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _societyService.Read(id);
            return Ok(Mapper.Map<SocietyViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]SocietyViewModel viewmodel)
        {
            var model = Mapper.Map<Society>(viewmodel);
            await _societyService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateSetting")]
        public async Task<IHttpActionResult> UpdateSetting([FromBody]SocietyViewModel viewmodel)
        {
            var model = Mapper.Map<Society>(viewmodel);
            await _societyService.UpdateSetting(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _societyService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]SocietySearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<SocietySearchParams>(searchParams);
            var result = await _societyService.List(inParams);
            var viewmodels = new List<SocietyViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<SocietyViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Import")]
        public async Task<IHttpActionResult> Import([FromBody]SocietyViewModel[] viewmodels)
        {
            var models = new List<Society>();
            foreach (var item in viewmodels)
            {
                models.Add(Mapper.Map<Society>(item));
            }

            var result = await _societyService.Import(models);

            var returnList = new List<SocietyImportResultViewModel>();
            foreach (var item in result)
            {
                returnList.Add(Mapper.Map<SocietyImportResultViewModel>(item));
            }

            return Ok(returnList);
        }

        [HttpPost]
        [Route("FlatCount")]
        public async Task<IHttpActionResult> FlatCount([FromBody] int id)
        {
            var result = await _societyService.FlatCount(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateLoginDetails")]
        public async Task<IHttpActionResult> UpdateLoginDetails([FromBody]SocietyViewModel viewmodel)
        {
            var model = Mapper.Map<Society>(viewmodel);
            await _societyService.UpdateLoginDetails(model);

            return Ok();
        }
    }
}