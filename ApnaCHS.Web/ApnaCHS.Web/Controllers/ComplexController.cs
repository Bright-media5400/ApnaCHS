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
    [RoutePrefix("api/Complex")]
    public class ComplexController : BaseController
    {
        private readonly IComplexService _complexService;
        public ComplexController()
            : base(new UserService())
        {
            _complexService = new ComplexService();
        }
        
        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]ComplexViewModel viewmodel)
        {
            var model = Mapper.Map<Complex>(viewmodel);
            var result = await _complexService.Create(model, CurrentUser);

            return Ok(Mapper.Map<ComplexViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _complexService.Read(id);
            return Ok(Mapper.Map<ComplexViewModel>(result));
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]ComplexViewModel viewmodel)
        {
            var model = Mapper.Map<Complex>(viewmodel);
            await _complexService.Update(model);

            return Ok();
        }
        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _complexService.Delete(id);
            return Ok();
        }
        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]ComplexSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<ComplexSearchParams>(searchParams);
            var result = await _complexService.List(inParams);
            var viewmodels = new List<ComplexViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<ComplexViewModel>(item));
            }

            return Ok(viewmodels);
        }
        [HttpPost]
        [Route("ReadArea")]
        public async Task<IHttpActionResult> ReadArea([FromBody] int pincode)
        {
            var result = await _complexService.ReadArea(pincode);

            var viewmodels = new List<AllIndiaPincodeViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<AllIndiaPincodeViewModel>(item));
            }

            return Ok(viewmodels);
        }
        [HttpPost]
        [Route("UpdateLoginDetails")]
        public async Task<IHttpActionResult> UpdateLoginDetails([FromBody]ComplexViewModel viewmodel)
        {
            var model = Mapper.Map<Complex>(viewmodel);
            await _complexService.UpdateLoginDetails(model);

            return Ok();
        }
    }
}