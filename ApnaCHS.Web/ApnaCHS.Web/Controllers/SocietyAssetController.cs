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
    [RoutePrefix("api/SocietyAsset")]
    public class SocietyAssetController : BaseController
    {
        private readonly ISocietyAssetService _societyAssetService;

        public SocietyAssetController()
            : base(new UserService())
        {
            _societyAssetService = new SocietyAssetService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]SocietyAssetViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyAsset>(viewmodel);
            var result = await _societyAssetService.Create(model);

            return Ok(Mapper.Map<SocietyAssetViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _societyAssetService.Read(id);
            return Ok(Mapper.Map<SocietyAssetViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]SocietyAssetViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyAsset>(viewmodel);
            await _societyAssetService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _societyAssetService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]SocietyAssetSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<SocietyAssetSearchParams>(searchParams);
            var result = await _societyAssetService.List(inParams);
            var viewmodels = new List<SocietyAssetViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<SocietyAssetViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}