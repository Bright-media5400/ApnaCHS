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
    [RoutePrefix("api/MaintenanceCostDefinition")]
    public class MaintenanceCostDefinitionController : BaseController
    {
        private readonly IMaintenanceCostDefinitionService _maintenanceCostDefinitionService;

        public MaintenanceCostDefinitionController()
            : base(new UserService())
        {
            _maintenanceCostDefinitionService = new MaintenanceCostDefinitionService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]MaintenanceCostDefinitionViewModel viewmodel)
        {
            var model = Mapper.Map<MaintenanceCostDefinition>(viewmodel);
            var result = await _maintenanceCostDefinitionService.Create(model);

            return Ok(Mapper.Map<MaintenanceCostDefinitionViewModel>(result));
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _maintenanceCostDefinitionService.Read(id);
            return Ok(Mapper.Map<MaintenanceCostDefinitionViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]MaintenanceCostDefinitionViewModel viewmodel)
        {
            var model = Mapper.Map<MaintenanceCostDefinition>(viewmodel);
            await _maintenanceCostDefinitionService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _maintenanceCostDefinitionService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]MaintenanceCostDefinitionSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<MaintenanceCostDefinitionSearchParams>(searchParams);
            var result = await _maintenanceCostDefinitionService.List(inParams);
            var viewmodels = new List<MaintenanceCostDefinitionViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<MaintenanceCostDefinitionViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}