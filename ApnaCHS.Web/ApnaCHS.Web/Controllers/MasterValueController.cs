using ApnaCHS.AppCommon;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Controllers;
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
    //[Authorize(Roles = "Backend")]
    [RoutePrefix("api/MasterValue")]
    public class MasterValueController : BaseController
    {
        private readonly IMasterValueService _masterValueService;

        public MasterValueController()
            : base(new UserService())
        {
            _masterValueService = new MasterValueService();
        }

        [HttpPost]
        [Route("GetListActive")]
        public async Task<IHttpActionResult> GetListActive([FromBody]byte id)
        {
            var values = await _masterValueService.GetListActive((EnMasterValueType)id);
            var viewmodels = new List<MasterValueViewModel>();
            foreach (var item in values)
            {
                viewmodels.Add(Mapper.Map<MasterValueViewModel>(item));
            }
            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("DropDown")]
        public async Task<IHttpActionResult> DropDown([FromBody]byte id)
        {
            var values = await _masterValueService.DropDown((EnMasterValueType)id);
            var viewmodels = new List<MasterValueViewModel>();
            foreach (var item in values)
            {
                viewmodels.Add(Mapper.Map<MasterValueViewModel>(item));
            }
            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("GetListAll")]
        public async Task<IHttpActionResult> GetListAll([FromBody]byte id)
        {
            var values = await _masterValueService.GetListAll((EnMasterValueType)id);
            var viewmodels = new List<MasterValueViewModel>();
            foreach (var item in values)
            {
                viewmodels.Add(Mapper.Map<MasterValueViewModel>(item));
            }
            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("NewMasterValue")]
        public async Task<IHttpActionResult> NewMasterValue(MasterValueViewModel model)
        {
            var masterVal = Mapper.Map<MasterValue>(model);
            await _masterValueService.New(masterVal);
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateMasterValue")]
        public async Task<IHttpActionResult> UpdateMasterValue(MasterValueViewModel model)
        {
            var masterVal = Mapper.Map<MasterValue>(model);
            await _masterValueService.Update(masterVal);
            return Ok(model);
        }

        [HttpPost]
        [Route("DeleteMasterValue")]
        public async Task<IHttpActionResult> DeleteMasterValue(MasterValueViewModel model)
        {
            var masterVal = Mapper.Map<MasterValue>(model);
            await _masterValueService.Delete(masterVal);
            return Ok();
        }

        [HttpPost]
        [Route("ActiveMasterValue")]
        public async Task<IHttpActionResult> ActiveMasterValue(MasterValueViewModel model)
        {
            await _masterValueService.Active(model.id);
            return Ok();
        }
    }
}