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
    [RoutePrefix("api/SocietyDocument")]
    public class SocietyDocumentController : BaseController
    {
        private readonly ISocietyDocumentService _societyDocumentService;

        public SocietyDocumentController()
            : base(new UserService())
        {
            _societyDocumentService = new SocietyDocumentService();
        }

            [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]SocietyDocumentViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyDocument>(viewmodel);
            var result = await _societyDocumentService.Create(model);

            return Ok(Mapper.Map<SocietyDocumentViewModel>(result));
        }

            [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _societyDocumentService.Read(id);
            return Ok(Mapper.Map<SocietyDocumentViewModel>(result));
        }

             [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]SocietyDocumentViewModel viewmodel)
        {
            var model = Mapper.Map<SocietyDocument>(viewmodel);
            await _societyDocumentService.Update(model);

            return Ok();
        }

             [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _societyDocumentService.Delete(id);
            return Ok();
        }

            [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]SocietyDocumentSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<SocietyDocumentSearchParams>(searchParams);
            var result = await _societyDocumentService.List(inParams);
            var viewmodels = new List<SocietyDocumentViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<SocietyDocumentViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}