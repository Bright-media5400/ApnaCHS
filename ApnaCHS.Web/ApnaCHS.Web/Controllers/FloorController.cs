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
    [RoutePrefix("api/Floor")]
    public class FloorController : BaseController
    {
        private readonly IFloorService _floorService;

        public FloorController()
            : base(new UserService())
        {
            _floorService = new FloorService();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create([FromBody]FloorViewModel viewmodel)
        {
            var model = Mapper.Map<Floor>(viewmodel);
            var result = await _floorService.Create(model);

            return Ok(Mapper.Map<FloorViewModel>(result));
        }

        [HttpPost]
        [Route("CreateMultiple")]
        public async Task<IHttpActionResult> CreateMultiple([FromBody]dynamic viewmodel)
        {
            FloorViewModel[] floors = viewmodel.floors.ToObject<FloorViewModel[]>();

            var model = new List<Floor>();
            foreach (var item in floors)
            {
                model.Add(Mapper.Map<Floor>(item));
            }

            await _floorService.CreateMultiple(model.ToArray());
            return Ok();
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _floorService.Read(id);
            return Ok(Mapper.Map<FloorViewModel>(result));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]FloorTrimViewModel viewmodel)
        {
            var model = Mapper.Map<Floor>(viewmodel);
            await _floorService.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody] int id)
        {
            await _floorService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]FloorSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FloorSearchParams>(searchParams);
            var result = await _floorService.List(inParams);
            var viewmodels = new List<FloorTrimViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FloorTrimViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("SocietyWiseList")]
        public async Task<IHttpActionResult> SocietyWiseList([FromBody]FloorSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<FloorSearchParams>(searchParams);
            var result = await _floorService.SocietyWiseList(inParams);
            var viewmodels = new List<FloorTrimViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<FloorTrimViewModel>(item));
            }

            return Ok(viewmodels);
        }

    }
}